# Module 3: MongoDB Node.js and C# Language Drivers

# Objectives

By the end of this section you will know how to:

- Connect a Node.js and C# application to MongoDB using a language driver
- Perform Create Read Update and Delete (CRUD) operations on a MongoDB collection from Node.js and C#

# Introduction

Every database has language drivers that allow you to interact with the database from a particular programming language. In this module we will use the Node.js and C# language drivers which allow us to do pretty much everything we are able to on the Mongo Interactive Shell but from our application.


# Node.js

## Getting Started

To install the MongoDB driver for Node.js execute the following command in the [Start](./Start) directory of this folder.

```batch
npm install
```

Because ```package.json``` contains the reference to the mongodb language driver, it gets installed as part of this command.

Now, let's take a look at our starter code within ```app.js```:

```js
// import the language driver
var MongoClient = require('mongodb').MongoClient
  , assert = require('assert');
 var ObjectID = require('mongodb').ObjectID;
  // Connection URL
var url = 'mongodb://127.0.0.1:27017/test';

// Use connect method to connect to the Server
MongoClient.connect(url, function(err, db) {
  
  //ensure we've connected
  assert.equal(null, err);
  

  console.log("Connected correctly to server");


  //close the database connection
  return db.close();

});
```

Running sample above by executing the command:

```batch
node app.js
```

(note: on some systems like Ubuntu the node binary is called 'nodejs'')

The output should indicate a successful connection to your local mongodb server:

```
Connected correctly to server
```

## Inserting a new Document
Now that we can connect to the server lets try inserting a new document into the Person collection. To begin interacting with our Person collection, we'll need to first request the collection reference from the db object.

```js
var bankData = db.collection('bank_data');
```

Then we can start inserting into this collection reference by simply specifying the object to be inserted into the collection:

```

bankData.insert({
  	first_name: "Steven",
  	last_name: "Edouard",
  	accounts: [
  	{
  		account_balance: "50000000",
  		account_type: "Investment",
  		currency: "USD"
  	}]
  }) 

```

Finally because Node.js is completely asynchronous, we'll have to add a callback which will be executed once the insert is completed:

```

bankData.insert({
  	first_name: "Steven",
  	last_name: "Edouard",
  	accounts: [
  	{
  		account_balance: "50000000",
  		account_type: "Investment",
  		currency: "USD"
  	}]
  }, function(err, result){
  	if(err){
  		return console.error(err);
  	}

  	console.log('inserted: ');
  	console.log(result);
  	return console.log('inserted ' + result.length + ' docs');  }) 

```

Putting everything together to insert the document to our collection:

```js
// import the language driver
var MongoClient = require('mongodb').MongoClient
  , assert = require('assert');
  // Connection URL for local mongodb server
var url = 'mongodb://127.0.0.1:27017/test';

// Use connect method to connect to the Server
MongoClient.connect(url, function(err, db) {
  
  //ensure we've connected
  assert.equal(null, err);
  
  var bankData = db.collection('bank_data');
  
  //insert new document to bank_data collection
  bankData.insert({
  	first_name: "Steven",
  	last_name: "Edouard",
  	accounts: [
  	{
  		account_balance: "50000000",
  		account_type: "Investment",
  		currency: "USD"
  	}]
  }, function(err, result){
      //callback is executed after mongodb confirms insert
  	if(err){
  		return console.error(err);
  	}

  	console.log('inserted: ');
  	console.log(result);
  	console.log('inserted ' + result.length + ' docs');
  	
	return db.close();

  });
});
```

Running the above code against your database should result in output similar to the following snippet:

```
Connected correctly to server
inserted: 
[ { first_name: 'Steven',
    last_name: 'Edouard',
    accounts: [ [Object] ],
    _id: 5469be33814972ee25b0aa9d } ]
inserted 1 docs
```

## Updating a stored document

We've successfully uploaded a person document, now let's modify the existing document that we've inserted. The above code returns the inserted document upon insert so all we need to to is use the **update** function to update the document in the database.

```js

var updatedPerson = result[0];
//increment this persons balance by 100000
updatedPerson.accounts[0].account_balance += 100000;

bankData.update( { _id: new ObjectID(updatedPerson._id )}, updatedPerson, {w: 1}, function(err, count){
	  	if(err){
  			return console.error(err);
  		}

  		console.log('successfully updated ' + count + ' person documents');

});

```
The callback will receive any error from mongodb and the count of documents that were updated.

The output should look something similar to:

```
sucessfully updated 1 person documents
```
## Reading a stored document

Now that we've successfully inserted and updated a document, let's try retrieving that document. This is very similar to the **find** exercises from module 2 inside the interactive shell. We can query using the node.js driver's **findOne** function to find the document we just inserted. 

FindOne is a convenience function to return the first document from a query - this is useful for queries where you only expect a single document. Let's query by the unique _id field:

```js
//retrieve the inserted collection from mongodb
//should be the exact same object we just updated
	  	
bankData.findOne({_id: new ObjectID(result[0]._id)}, function(err,doc){

	if(err){
		return console.error(err);
	}

	console.log('retrieved Person ' + doc.first_name + ' ' + doc.last_name);
	console.log('accounts: ');
	for(var i in doc.accounts){
		console.log('Type: ' + doc.accounts[i].account_type);
		console.log('Balance: ' + doc.accounts[i].account_balance);
	}
	
	return db.close();
});
```

The fetched single document is returned to the callback and we can pull out any appropriate fields just as a native javascript object. The output of above should be:

```
retrieved Person Steven Edouard
accounts: 
Type: Investment
Balance: 50000000100000
```

### Deleting a Stored Document

Finally, to finish our walkthrough on CRUD operations from Node.js, let's delete the record  we've inserted. We can do this by calling the **remove** function on the collection object.

The drop function takes the same type of query projection object as the **find** and **update** functions.

We'll use the unique **_id** field to delete the exact record we just inserted and updated:

```js
//now delete the document we just inserted

bankData.remove({_id: new ObjectID(result[0]._id)}, function(err,count){
	if(err){
		db.close();
		return console.error(err);
	}

	console.log('sucessfully deleted ' + count + ' documents');

	return db.close();
});
```

The output of this should just be:

```
sucessfully deleted 1 documents
```

## Putting it all together

In the above snippets we've gone through each type of CRUD operation. We've created a new person document, updated that document, read it and then deleted it.

Putting all these pieces together in order requires us to interleave the calls within each callback. In Node.js development there are libraries that implement [promises]() or futures that help make your code more linear looking.

```js
  
  var bankData = db.collection('bank_data');

  bankData.insert({
  	first_name: "Steven",
  	last_name: "Edouard",
  	accounts: [
  	{
  		account_balance: "50000000",
  		account_type: "Investment",
  		currency: "USD"
  	}]
  }, function(err, result){

  	if(err){
  		return console.error(err);
  	}

  	console.log('inserted: ');
  	console.log(result);
  	console.log('inserted ' + result.length + ' docs');

  	var updatedPerson = result[0];
  	updatedPerson.accounts[0].account_balance += 100000;

  	bankData.update( { _id: new ObjectID(result[0]._id )}, updatedPerson, {w: 1}, function(err, count){

  		if(err){
  			return console.error(err);
  		}

  		console.log('sucessfully updated ' + count + ' person documents');

  		//retrieve the inserted collection from mongodb
	  	//should be the exact same object we just updated
	  	
	  	bankData.findOne({_id: new ObjectID(result[0]._id)}, function(err,doc){

	  		if(err){
	  			return console.error(err);
	  		}

	  		console.log('retrieved Person ' + doc.first_name + ' ' + doc.last_name);
	  		console.log('accounts: ');
	  		for(var i in doc.accounts){
	  			console.log('Type: ' + doc.accounts[i].account_type);
	  			console.log('Balance: ' + doc.accounts[i].account_balance);
	  		}

	  		//now delete the document we just inserted

	  		bankData.remove({_id: new ObjectID(result[0]._id)}, function(err,count){

	  			if(err){
	  				db.close();
	  				return console.error(err);
	  			}

	  			console.log('sucessfully deleted ' + count + ' documents');

	  			return db.close();
	  		});

	  		
	  	});
  	});
  	return;
  });
```

Now we'll go through what the functional equivalent of this code looks like in C#.

# C# and .NET

<RAMI, please do the equivalent from above except in C# >

