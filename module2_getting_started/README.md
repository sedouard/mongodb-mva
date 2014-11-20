# Getting Started With MongoDB

## Objectives

By the end of this module you will know how to:

- Install, setup and run MongoDB on your local machine
- Load bulk data from a text file into MongoDB using mongoimport
- Use the Interactive Shell to run basic queries


## Getting Started

The first thing you need to do is [install mongodb](http://docs.mongodb.org/manual/installation/) for your specific system. Follow the instructions for mongodb for your operating system.

You should setup mongodb such that you have it installed in an easy to access directory such as 

```bash
# linux/unix
/mongodb
# windows
C:\mongodb
```

Now that you have MongoDB installed you need to create a data directory for it to store data to the disk. Execute the following commands from your **bin** folder in your mongodb installation:

```bash
mkdir /data/db
```
Now that we have our data directory you can run the mongodb database process 

**mongod**:

```bash
mongod
```

This starts up the MongoDB server and indicates we can start inserting data.

## MongoDB Database Organization

Similar to other database systems, Mongodb data organization starts with the database server (the **mongod** process. Each server may have N number of databases and within each database there are N number of [collections](http://docs.mongodb.org/manual/reference/glossary/#term-collection). 

![](ScreenShots/mongodbss2.png)

This structure allows for more advance scaling such as [sharding](http://docs.mongodb.org/manual/sharding/) to provide more throughput and [replicase sets](http://docs.mongodb.org/manual/core/replication/) for better redundancy.

## Loading up test data into mongodb

Within the data directory of this repository there is a **bank_data.json.zip** file; you should decompress it. We will use this file to load up the bank data collection into mongodb. From the mongodb installation **bin** in a new console window, execute the command:

```bash
mongoimport <path to bank_data.json> --jsonArray --collection bank_data
```
The command will actually load up the json data in the file up into a collection called 'bank_data' in your default MongoDB database on your mongodb server. This may take a while however you'll be able to keep track of the percentage:

```bash
Users-Computer-4:bin user$ ./mongoimport /Users/user/Documents/crimeDataConvert/data/bank_data.json --jsonArray --collection bank_data
connected to: 127.0.0.1
2014-11-16T02:11:10.797-0800 		Progress: 192824/22758676	0%
2014-11-16T02:11:10.797-0800 			400	133/second
2014-11-16T02:11:13.316-0800 		Progress: 340537/22758676	1%
2014-11-16T02:11:13.316-0800 			700	116/second
2014-11-16T02:11:16.648-0800 		Progress: 533438/22758676	2%
2014-11-16T02:11:16.648-0800 			1100	122/second
```

After all of this 50,000 documents will be uploaded into your local MongoDB server. Although that may sound like a lot, its likely a manageable size to work with on your computer.

Now that all your bank accounts data has been loaded up into MongoDB we are ready to start exploring!

## The MongoDB Interactive Shell

MongoDB has an interactive shell which is very similar to the javascript interactive shell except it includes a mongosb api. Most commands are actually javascript however are slightly different than the Node.js javascript version we will work with in [module 3](../module3_language_drivers/README.md). To run the shell execute the following command from your mongodb installation **bin** folder:

```bash
mongo
```

This will automatically connect you to your local mongodb server running in your other console window.

If you ever want to explore anything outside of this guide in the interactive shell, you can get more info by doing:

```bash
help
```

The remainder of this module will use the MongoDB Interactive Shell to explore MongoDB capabilities. [Module 3](../module3_language_drivers/README.md) will focus on the [C# (http://docs.mongodb.org/ecosystem/drivers/csharp/)and [Node.js](http://docs.mongodb.org/ecosystem/drivers/node-js/) language drivers.

### Collections

Collections in in MongoDB are containers for a group of similar entities. In our example from module1 the **Person** entity was the root entity. A single database can have many collections. To display the collections in your database execute:

```
# shows collections in database
show collections
```

The output of your command will look similar to:

```bash
> show collections
bank_data
system.indexes

```

To get the collection object, we can use the **db** reference to the current database.

```bash
db.bank_data
```
To see how many documents we loaded into the mongodb collection we can use the **count()** function on the collection object:

```bash
> db.bank_data.count()
50000
```

There's much more you can do with a collection. Checkout the collections help by doing:

```bash
db.bank_data.help()
```

### Queries

Now that we have bulk data loaded up we can make some queries across the Person collection. All queries in MongoDB use the **find** function on the query to get specific documents from the collection. Let's try our first query with the .findOne method which finds the first document in the collection.

```js
db.bank_data.findOne()
```

This command should return something similar to:

```json
{
	"_id" : ObjectId("5468782fca357ca95f2f4b88"),
	"first_name" : "JAMES",
	"last_name" : "SMITH",
	"accounts" : [
		{
			"account_type" : "Investment",
			"account_balance" : 6144974.110823463,
			"currency" : "YEN"
		},
		{
			"account_type" : "Savings",
			"account_balance" : 1329372.569229168,
			"currency" : "EURO"
		}
	]
}
```

Like our example from [module 1](../module1_intro_doc_dbs/README.md) we get a Person document with a number of Account sub documents within it. This query didn't really have anything interesting to it. We simply wanted to retrieve the first person in Person collection (that is what MongoDB interpreted when we didn't specify any parameters into the *findOne()* function).

Let's see what happens when you execute:

```js
db.collection.find()
```

You'll actually get a ton of text similar to:

```json

{ "_id" : ObjectId("5468782fca357ca95f2f4b88"), "first_name" : "JAMES", "last_name" : "SMITH", "accounts" : [ { "account_type" : "Investment", "account_balance" : 6144974.110823463, "currency" : "YEN" }, { "account_type" : "Savings", "account_balance" : 1329372.569229168, "currency" : "EURO" } ] }
{ "_id" : ObjectId("54687830ca357ca95f2f4b89"), "first_name" : "JAMES", "last_name" : "JOHNSON", "accounts" : [ { "account_type" : "Investment", "account_balance" : 9840052.941285312, "currency" : "POUNDS STERLING" }, { "account_type" : "401K", "account_balance" : 5588991.2251258055, "currency" : "PESO" }, { "account_type" : "Savings", "account_balance" : 4235557.4669413585, "currency" : "POUNDS STERLING" }, { "account_type" : "Savings", "account_balance" : 8799224.831341146, "currency" : "YEN" }, { "account_type" : "401K", "account_balance" : 5962745.686764822, "currency" : "USD" }, { "account_type" : "Savings", "account_balance" : 9818556.57020622, "currency" : "POUNDS STERLING" }, { "account_type" : "Investment", "account_balance" : 6418584.010852241, "currency" : "EURO" } ] }
{ "_id" : ObjectId("54687830ca357ca95f2f4b8a"), "first_name" : "JAMES", "last_name" : "WILLIAMS", "accounts" : [ { "account_type" : "Investment", "account_balance" : 4904214.484536952, "currency" : "YEN" }, { "account_type" : "Savings", "account_balance" : 8618841.25467285, "currency" : "YUAN" }, { "account_type" : "Investment", "account_balance" : 913765.1830773808, "currency" : "YUAN" }, { "account_type" : "Checking", "account_balance" : 1376331.8743719491, "currency" : "PESO" }, { "account_type" : "Investment", "account_balance" : 7871363.623830194, "currency" : "PESO" }, { "account_type" : "Checking", "account_balance" : 6203323.441020354, "currency" : "YUAN" }, { "account_type" : "Investment", "account_balance" : 917203.6349922416, "currency" : "USD" }, { "account_type" : "401K", "account_balance" : 5979093.451052592, "currency" : "PESO" } ] }
{ "_id" : ObjectId("54687830ca357ca95f2f4b8b"), "first_name" : "JAMES", "last_name" : "BROWN", "accounts" : [ { "account_type" : "Checking", "account_balance" : 5155.384490680648, "currency" : "EURO" } ] }
{ "_id" : ObjectId("54687830ca357ca95f2f4b8c"), "first_name" : "JAMES", "last_name" : "JONES", "accounts" : [ { "account_type" : "Savings", "account_balance" : 1871922.5181302335, "currency" : "POUNDS STERLING" }, { "account_type" : "401K", "account_balance" : 2454099.221148599, "currency" : "YUAN" }, { "account_type" : "Checking", "account_balance" : 8873605.096902523, "currency" : "PESO" }, { "account_type" : "Checking", "account_balance" : 7345421.697284348, "currency" : "POUNDS STERLING" }, { "account_type" : "Savings", "account_balance" : 6429599.448691073, "currency" : "EURO" }, { "account_type" : "Savings", "account_balance" : 6569446.0432147905, "currency" : "YUAN" }, { "account_type" : "401K", "account_balance" : 2952708.430080968, "currency" : "EURO" } ] }
{ "_id" : ObjectId("54687830ca357ca95f2f4b8d"), "first_name" : "JAMES", "last_name" : "MILLER", "accounts" : [ { "account_type" : "Checking", "account_balance" : 5000794.5164514, "currency" : "EURO" } ] }
{ "_id" : ObjectId("54687830ca357ca95f2f4b8e"), "first_name" : "JAMES", "last_name" : "DAVIS", "accounts" : [ { "account_type" : "401K", "account_balance" : 1634239.7192724317, "currency" : "EURO" }, { "account_type" : "Checking", "account_balance" : 5123875.592675053, "currency" : "YEN" }, { "account_type" : "Investment", "account_balance" : 9652688.997844521, "currency" : "PESO" }, { "account_type" : "401K", "account_balance" : 436608.31229717913, "currency" : "YUAN" } ] }
{ "_id" : ObjectId("54687830ca357ca95f2f4b8f"), "first_name" : "JAMES", "last_name" : "GARCIA", "accounts" : [ { "account_type" : "Investment", "account_balance" : 3928991.3192649623, "currency" : "YUAN" }, { "account_type" : "Checking", "account_balance" : 4061586.788287755, "currency" : "YUAN" }, { "account_type" : "Investment", "account_balance" : 3907978.032141935, "currency" : "POUNDS STERLING" }, { "account_type" : "Checking", "account_balance" : 7980062.845765791, "currency" : "EURO" }, { "account_type" : "Investment", "account_balance" : 5629389.621828526, "currency" : "USD" }, { "account_type" : "Savings", "account_balance" : 2822423.1437263936, "currency" : "PESO" }, { "account_type" : "401K", "account_balance" : 5603218.9455431765, "currency" : "EURO" } ] }
```

You'll see the ```Type "it" for more``` prompt because the **find()** method called without any parameters retrieves ALL documents in the collection. Since you have so many documents in the collection MongoDB will give us a [cursor](http://en.wikipedia.org/wiki/Cursor_(databases)) which will allow us to iterate through the results in a more manageable fashion.

We can always do a **count()** over the results to see how many documents were retrieved as a result of the query. For example:

```js
db.bank_data.find().count()
```

```json
50000
```

Will return the number of documents in the collection since *find()* has no parameters.

Let's look at some of basic queries and those with **AND**, **OR**, **Greater Than/Less Than/NotEqual**:

### Basic Queries with Filters

The **find** function takes sort of an object projection to return the desired set of documents. For example, to return all Person documents with the last name = "Smith" we can do:

```js
db.bank_data.find({last_name : "SMITH"}).count()
```

And we'll see how many people exist in the database with the last name, SMITH:

```
100
```

We can pick out a record to display by selecting a number on the collection between 0 and 100 for this example:

```js
db.bank_data.find({last_name : "SMITH"})[12]
```
	
This will return the 13th record in the result:

```json
{
	"_id" : ObjectId("546960b6ca357ca95f301689"),
	"first_name" : "CHRISTOPHER",
	"last_name" : "SMITH",
	"accounts" : [
		{
			"account_type" : "Savings",
			"account_balance" : 1870338.5807330757,
			"currency" : "POUNDS STERLING"
		},
		{
			"account_type" : "Checking",
			"account_balance" : 5098043.412530517,
			"currency" : "POUNDS STERLING"
		},
		{
			"account_type" : "Checking",
			"account_balance" : 3042158.5783834355,
			"currency" : "YEN"
		},
		{
			"account_type" : "Investment",
			"account_balance" : 2721431.0181220435,
			"currency" : "PESO"
		},
		{
			"account_type" : "Savings",
			"account_balance" : 2135388.570106006,
			"currency" : "USD"
		},
		{
			"account_type" : "Savings",
			"account_balance" : 2416831.8654360515,
			"currency" : "PESO"
		},
		{
			"account_type" : "Savings",
			"account_balance" : 405349.36727577704,
			"currency" : "POUNDS STERLING"
		},
		{
			"account_type" : "Investment",
			"account_balance" : 6140519.315997784,
			"currency" : "YEN"
		}
	]
}

```

Notice how we retrieved all the accounts as well, even though we only specified the persons last name. This is because when we query we are querying for documents and therefore any associated information is also retrieved.

It's possible to omit the associated information by specifying a second projection object to the *find()* function:

```js
db.bank_data.find({last_name : "SMITH"}, {first_name : 1, last_name: 1})[12]
```

```json
{
	"_id" : ObjectId("546960b6ca357ca95f301689"),
	"first_name" : "CHRISTOPHER",
	"last_name" : "SMITH"
}
```

Notice how the output above omits all the accounts for CHRISTOPHER SMITH.

We can also output the names of each person who came in the result set pretty easily by iterating through the result collection by saving it to a variable

```js
var smithPersons = db.bank_data.find({last_name : "SMITH"}, {first_name : 1, last_name: 1});
for(var i = 0; i < smithPersons.count(); i++){ 
	print(smithPersons[i].first_name + ' ' + smithPersons[i].last_name );
}
```
You'll see a long list of output similar to the following:

```
JAMES SMITH
JOHN SMITH
ROBERT SMITH
MICHAEL SMITH
WILLIAM SMITH
DAVID SMITH
...
```
### AND & OR Queries

#### AND Queries

We can achieve an AND query by simply specifying another field in the project object we pass to the *find()* function. Say that we want to get everyone in the database whose last name is SMITH and first name is JAMES:
	
```js
db.bank_data.find({last_name: "SMITH", first_name: "JAMES"})
```

We should just get one result if you haven't modified the test data (you can get the pretty JSON formatting by just doing ''[0]" on the collection):

```json
{
	"_id" : ObjectId("546960b6ca357ca95f30167f"),
	"first_name" : "JAMES",
	"last_name" : "SMITH",
	"accounts" : [
		{
			"account_type" : "Savings",
			"account_balance" : 8995952.153640702,
			"currency" : "PESO"
		},
		{
			"account_type" : "Checking",
			"account_balance" : 3901436.5580737568,
			"currency" : "USD"
		}
	]
}
```

We can also specify these constraints on embedded collections.

First, let's look for all Persons whose last name is SMITH  *and* have a Savings account:

```js
//we know that we have more than 12 elements in the resulting collection
db.bank_data.find({last_name: "SMITH", "accounts.account_type": "Savings" })[12]
```
```json
{
	"_id" : ObjectId("546960b6ca357ca95f301691"),
	"first_name" : "EDWARD",
	"last_name" : "SMITH",
	"accounts" : [
		{
			"account_type" : "401K",
			"account_balance" : 6577381.434625924,
			"currency" : "YEN"
		},
		{
			"account_type" : "Checking",
			"account_balance" : 8998935.759297138,
			"currency" : "USD"
		},
		{
			"account_type" : "Investment",
			"account_balance" : 588000.4045217587,
			"currency" : "YUAN"
		},
		{
			// This is the embedded document that matched the query
			"account_type" : "Savings",
			"account_balance" : 6743563.754405159,
			"currency" : "YUAN"
		},
		{
			"account_type" : "401K",
			"account_balance" : 8580650.627761671,
			"currency" : "POUNDS STERLING"
		},
		{
			"account_type" : "401K",
			"account_balance" : 7687815.685781645,
			"currency" : "YEN"
		},
		{
			"account_type" : "Checking",
			"account_balance" : 9128407.633252997,
			"currency" : "EURO"
		}
	]
}
```

Notice how we retrieve the entire ```accounts``` array even though we only were looking for the element with the ```account_type``` of Savings. The [```$elemMatch```](http://docs.mongodb.org/manual/reference/operator/projection/elemMatch/) projection operator allows us to return just the first element in an array that meets the criteria. 
	We can use this operator by passing a second projection object which filters the information returned by MongoDB:

```js

// only get the first matching sub document in the array ''accounts'
db.bank_data.find({last_name: "SMITH", "accounts.account_type": "Savings" }, { first_name: 1, last_name: 1, accounts: { $elemMatch : { 'account_type' : 'Savings' } } } )[12]
```

This will give us something similar to:

```json
{
	"_id" : ObjectId("546d7827df1e5b91fcb57133"),
	"first_name" : "EDWARD",
	"last_name" : "SMITH",
	"accounts" : [
		{
			"account_type" : "Savings",
			"account_balance" : 6743563.754405159,
			"currency" : "YUAN"
		}
	]
}
```

In the query above we get ```first_name```, ```last_name``` and only the first matching ```account```. This is great way to limit the amount of data sent back by MongoDB which could be helpful for performance reasons.

#### OR Queries

OR queries are slightly different because they require us to use the **$or** operator alias, since the projection object syntax doesn't allow for us to specify it any other way.

Say that we wanted to get any person whose last name is SMITH or MARTINEZ. This query would look something like:

```js

db.bank_data.find({$or: [ { last_name: "MARTINEZ"}, {last_name: "SMITH"} ]})

```

To make your output a little cleaner, lets omit the accounts data:

```js

db.bank_data.find({$or: [ { last_name: "MARTINEZ"}, {last_name: "SMITH"} ]}, {first_name: 1, last_name: 1})

```
	
You should get a bunch of Person documents with either their last name being MARTINEZ or last name being SMITH


```json

{ "_id" : ObjectId("546960b7ca357ca95f3016cf"), "first_name" : "CARLOS", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960b7ca357ca95f3016d0"), "first_name" : "RUSSELL", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960b7ca357ca95f3016d1"), "first_name" : "BOBBY", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960b7ca357ca95f3016d2"), "first_name" : "VICTOR", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960b7ca357ca95f3016d3"), "first_name" : "MARTIN", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960b7ca357ca95f3016d4"), "first_name" : "ERNEST", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960b7ca357ca95f3016d5"), "first_name" : "PHILLIP", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960b7ca357ca95f3016d6"), "first_name" : "TODD", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960b7ca357ca95f3016d7"), "first_name" : "JESSE", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960b7ca357ca95f3016d8"), "first_name" : "CRAIG", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960b7ca357ca95f3016d9"), "first_name" : "ALAN", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960b7ca357ca95f3016da"), "first_name" : "SHAWN", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960b7ca357ca95f3016db"), "first_name" : "CLARENCE", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960b7ca357ca95f3016dc"), "first_name" : "SEAN", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960b7ca357ca95f3016dd"), "first_name" : "PHILIP", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960b7ca357ca95f3016de"), "first_name" : "CHRIS", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960b7ca357ca95f3016df"), "first_name" : "JOHNNY", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960b7ca357ca95f3016e0"), "first_name" : "EARL", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960b7ca357ca95f3016e1"), "first_name" : "JIMMY", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960b7ca357ca95f3016e2"), "first_name" : "ANTONIO", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960beca357ca95f301a67"), "first_name" : "JAMES", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960beca357ca95f301a68"), "first_name" : "JOHN", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960beca357ca95f301a69"), "first_name" : "ROBERT", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960beca357ca95f301a6a"), "first_name" : "MICHAEL", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960beca357ca95f301a6b"), "first_name" : "WILLIAM", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960beca357ca95f301a6c"), "first_name" : "DAVID", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960beca357ca95f301a6d"), "first_name" : "RICHARD", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960beca357ca95f301a6e"), "first_name" : "CHARLES", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960beca357ca95f301a6f"), "first_name" : "JOSEPH", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960beca357ca95f301a70"), "first_name" : "THOMAS", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960beca357ca95f301a71"), "first_name" : "CHRISTOPHER", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960beca357ca95f301a72"), "first_name" : "DANIEL", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960beca357ca95f301a73"), "first_name" : "PAUL", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960beca357ca95f301a74"), "first_name" : "MARK", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960beca357ca95f301a75"), "first_name" : "DONALD", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960beca357ca95f301a76"), "first_name" : "GEORGE", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960beca357ca95f301a77"), "first_name" : "KENNETH", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960beca357ca95f301a78"), "first_name" : "STEVEN", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960beca357ca95f301a79"), "first_name" : "EDWARD", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960beca357ca95f301a7a"), "first_name" : "BRIAN", "last_name" : "MARTINEZ" }

```

Note that **you may have to type 'it'** to iterate to get to the MARTINEZ results. You can also get the results ordered by first name to see that you are getting both last names by using the **sort()** function on the returned cursor

```js
db.bank_data.find({$or: [ { last_name: "MARTINEZ"}, {last_name: "SMITH"} ]}, {first_name: 1, last_name: 1}).sort({first_name: 1 })
```

The '1' means to sort ascending by the specified column, and '-1' means sort descending. Since these are string fields, you'll get a a nice alphabetical listing descending by first name:

```json
{ "_id" : ObjectId("546960b6ca357ca95f3016cb"), "first_name" : "AARON", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960bfca357ca95f301ab3"), "first_name" : "AARON", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960b6ca357ca95f3016c3"), "first_name" : "ADAM", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960bfca357ca95f301aab"), "first_name" : "ADAM", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960b7ca357ca95f3016d9"), "first_name" : "ALAN", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960bfca357ca95f301ac1"), "first_name" : "ALAN", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960b6ca357ca95f3016b4"), "first_name" : "ALBERT", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960bfca357ca95f301a9c"), "first_name" : "ALBERT", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960b6ca357ca95f3016a1"), "first_name" : "ANDREW", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960beca357ca95f301a89"), "first_name" : "ANDREW", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960b6ca357ca95f301694"), "first_name" : "ANTHONY", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960beca357ca95f301a7c"), "first_name" : "ANTHONY", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960b7ca357ca95f3016e2"), "first_name" : "ANTONIO", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960bfca357ca95f301aca"), "first_name" : "ANTONIO", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960b6ca357ca95f3016ae"), "first_name" : "ARTHUR", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960bfca357ca95f301a96"), "first_name" : "ARTHUR", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960b6ca357ca95f3016c0"), "first_name" : "BENJAMIN", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960bfca357ca95f301aa8"), "first_name" : "BENJAMIN", "last_name" : "MARTINEZ" }
{ "_id" : ObjectId("546960b6ca357ca95f3016c7"), "first_name" : "BILLY", "last_name" : "SMITH" }
{ "_id" : ObjectId("546960bfca357ca95f301aaf"), "first_name" : "BILLY", "last_name" : "MARTINEZ" }

```
### Greater Than, Less Than, Not Equal To Operators

Like the **$or** operator alias, mongodb also offers the **$gt**, **$lt**, **$ne** operator aliases for greater than, less than and equal to operators, respectivley. There are also coorsponding **$gte**, **$lte** for greater than or equal to and less than or equal to.

Say that we only wanted to retrieve persons with any bank account with an account balance greater than 9 million dollars USD.
	
We can do this by using the **$gt** operator on the embedded account document *account_balance* field:

```js
db.bank_data.find({ 'accounts.account_balance': {$gt: 9000000} })
```

Here's the 1000th returned Person document:

```json
{
	"_id" : ObjectId("546960cbca357ca95f302080"),
	"first_name" : "RALPH",
	"last_name" : "LEWIS",
	"accounts" : [
		{
			"account_type" : "Checking",
			"account_balance" : 5643915.251352968,
			"currency" : "PESO"
		},
		{
			"account_type" : "Savings",
			"account_balance" : 1344242.1501158585,
			"currency" : "USD"
		},
		{
			"account_type" : "Savings",
			"account_balance" : 1521705.760721184,
			"currency" : "POUNDS STERLING"
		},
		{
			"account_type" : "401K",
			"account_balance" : 9749745.140437951,
			"currency" : "YUAN"
		},
		{
			"account_type" : "Savings",
			"account_balance" : 7826270.383932192,
			"currency" : "POUNDS STERLING"
		}
	]
}
```

This still doesn't get us exactly where we want to be just yet since we aren't taking into consideration the *currency* field. It isn't accurate to query by just the account balance because different currencies are worth different amounts. We can combine $gt operator with an **and** operation by just specifying the second field in the sub document:

```js
db.bank_data.find({ 'accounts.account_balance': {$gt: 9000000}, 'account.currency': 'USD' })
```

Although this seems correct its actually subtly inaccurate. This query will return Persons with an account that has at least 9 Million and an account that has a currency type of USD. Unfortunatley that's not what we were looking for.

This problem can be solved with the [```$elemMatch```](http://docs.mongodb.org/manual/reference/operator/query/elemMatch/) query operator which can be used to match a specific element in an array embedded in the document. This operator works very similar to the [projection version](http://docs.mongodb.org/manual/reference/operator/query/elemMatch/) with the exception that it is used as part of a query and not a projection. 

Here's how we can use the operator to find a Persons with an account balance of $9 USD:

```js
db.bank_data.find({ accounts: { $elemMatch : { "account_type": "Checking", "currency": "USD", 'account_balance' : { $gt: 9900000 } } } } )
```

Now you can see that we are only selecting Persons with USD bank accounts greater than 9 million dollars.

```json
{
	"_id" : ObjectId("546960b7ca357ca95f301732"),
	"first_name" : "EUGENE",
	"last_name" : "JOHNSON",
	"accounts" : [
		{
			"account_type" : "Savings",
			"account_balance" : 9644738.165106645,
			"currency" : "YUAN"
		},
		{
			"account_type" : "Investment",
			"account_balance" : 3382188.763320642,
			"currency" : "YUAN"
		},
		{
			"account_type" : "Checking",
			"account_balance" : 7265775.6412804155,
			"currency" : "POUNDS STERLING"
		},
		{
			"account_type" : "401K",
			"account_balance" : 4662322.562082556,
			"currency" : "YEN"
		},
		{
			"account_type" : "Checking",
			"account_balance" : 1585816.9293256353,
			"currency" : "USD"
		},
		{
			///////////////////////////////////////////////////////
			// This is the matching document
			///////////////////////////////////////////////////////
			"account_type" : "Checking",
			"account_balance" : 9512844.082029575,
			"currency" : "USD"
		}
	]
}

```

What if the bank wanted to exclude all persons with a checking account front this query. We can do this with the **$ne** operator alias:

```js
db.bank_data.find({ 'accounts.account_balance': {$gt: 9000000}, 'account.currency': 'USD', 'accounts.account_type': {$ne: 'Checking' } })
```

In the sample of documents received, note there are no Checking accounts:

```json
{ "_id" : ObjectId("546960baca357ca95f301836"), "first_name" : "DENNIS", "last_name" : "JONES", "accounts" : [ { "account_type" : "Investment", "account_balance" : 1663319.9657335512, "currency" : "PESO" }, { "account_type" : "Savings", "account_balance" : 9165545.972862184, "currency" : "USD" }, { "account_type" : "Investment", "account_balance" : 587530.2496500439, "currency" : "POUNDS STERLING" }, { "account_type" : "Savings", "account_balance" : 4968418.510649419, "currency" : "YEN" }, { "account_type" : "Savings", "account_balance" : 9953368.745059764, "currency" : "PESO" } ] }
{ "_id" : ObjectId("546960baca357ca95f30183e"), "first_name" : "ARTHUR", "last_name" : "JONES", "accounts" : [ { "account_type" : "Investment", "account_balance" : 3257180.6435127337, "currency" : "POUNDS STERLING" }, { "account_type" : "Investment", "account_balance" : 4662125.037897458, "currency" : "PESO" }, { "account_type" : "Investment", "account_balance" : 9825512.559612762, "currency" : "EURO" }, { "account_type" : "401K", "account_balance" : 7437629.959444035, "currency" : "POUNDS STERLING" }, { "account_type" : "401K", "account_balance" : 6814759.514081009, "currency" : "YUAN" }, { "account_type" : "Savings", "account_balance" : 8037030.051490649, "currency" : "YEN" }, { "account_type" : "401K", "account_balance" : 4450354.054241277, "currency" : "USD" } ] }
{ "_id" : ObjectId("546960baca357ca95f301846"), "first_name" : "JUSTIN", "last_name" : "JONES", "accounts" : [ { "account_type" : "401K", "account_balance" : 5565130.27981727, "currency" : "YUAN" }, { "account_type" : "Savings", "account_balance" : 7471462.034394771, "currency" : "PESO" }, { "account_type" : "401K", "account_balance" : 3355240.8867829638, "currency" : "YUAN" }, { "account_type" : "401K", "account_balance" : 8534122.932381403, "currency" : "YEN" }, { "account_type" : "401K", "account_balance" : 1390575.4528828736, "currency" : "POUNDS STERLING" }, { "account_type" : "401K", "account_balance" : 7442115.797576328, "currency" : "EURO" }, { "account_type" : "401K", "account_balance" : 9704565.387666339, "currency" : "YEN" }, { "account_type" : "401K", "account_balance" : 7691654.466665171, "currency" : "USD" } ] }
```

We can also order our query by descending account balance with the **sort()** function like we did previously:

```js

db.bank_data.find({ 'accounts.account_balance': {$gt: 9000000}, 'account.currency': 'USD', 'accounts.account_type': {$ne: 'Checking' } }).sort({ accounts.account_balance: -1 })

```
	
```json
{ "_id" : ObjectId("54696170ca357ca95f30728f"), "first_name" : "ADAM", "last_name" : "FRANKLIN", "accounts" : [ { "account_type" : "Savings", "account_balance" : 2326953.599830377, "currency" : "POUNDS STERLING" }, { "account_type" : "Investment", "account_balance" : 9999989.46325779, "currency" : "YEN" }, { "account_type" : "Investment", "account_balance" : 2312229.915267023, "currency" : "USD" }, { "account_type" : "401K", "account_balance" : 8088697.964774987, "currency" : "YUAN" }, { "account_type" : "Investment", "account_balance" : 1475067.7595062084, "currency" : "EURO" }, { "account_type" : "Savings", "account_balance" : 8135675.7846087, "currency" : "USD" }, { "account_type" : "401K", "account_balance" : 2517044.483436625, "currency" : "USD" } ] }
{ "_id" : ObjectId("54696129ca357ca95f304c72"), "first_name" : "DANIEL", "last_name" : "TUCKER", "accounts" : [ { "account_type" : "Savings", "account_balance" : 1384388.1770525468, "currency" : "POUNDS STERLING" }, { "account_type" : "401K", "account_balance" : 683364.255003924, "currency" : "YEN" }, { "account_type" : "401K", "account_balance" : 5932435.368196609, "currency" : "YEN" }, { "account_type" : "Investment", "account_balance" : 8682883.11425823, "currency" : "USD" }, { "account_type" : "401K", "account_balance" : 9999904.172744935, "currency" : "YEN" } ] }
{ "_id" : ObjectId("546960deca357ca95f302985"), "first_name" : "FRED", "last_name" : "TURNER", "accounts" : [ { "account_type" : "Investment", "account_balance" : 9999788.563028697, "currency" : "USD" }, { "account_type" : "Investment", "account_balance" : 4000758.587309881, "currency" : "YEN" }, { "account_type" : "Savings", "account_balance" : 765218.2018713215, "currency" : "PESO" } ] }
{ "_id" : ObjectId("546961b3ca357ca95f30acd9"), "first_name" : "ALAN", "last_name" : "OCONNOR", "accounts" : [ { "account_type" : "401K", "account_balance" : 189080.1402718504, "currency" : "PESO" }, { "account_type" : "401K", "account_balance" : 5117138.140417096, "currency" : "PESO" }, { "account_type" : "Investment", "account_balance" : 379587.0013927594, "currency" : "PESO" }, { "account_type" : "Investment", "account_balance" : 3134700.8919473933, "currency" : "USD" }, { "account_type" : "Savings", "account_balance" : 6672982.434736467, "currency" : "USD" }, { "account_type" : "401K", "account_balance" : 9999633.20912556, "currency" : "PESO" }, { "account_type" : "Savings", "account_balance" : 4599414.107987304, "currency" : "PESO" } ] }
{ "_id" : ObjectId("546960faca357ca95f303639"), "first_name" : "KEVIN", "last_name" : "CRUZ", "accounts" : [ { "account_type" : "Savings", "account_balance" : 6700829.7476021685, "currency" : "YUAN" }, { "account_type" : "401K", "account_balance" : 5828740.46791689, "currency" : "USD" }, { "account_type" : "Investment", "account_balance" : 3725809.2336768657, "currency" : "USD" }, { "account_type" : "Savings", "account_balance" : 4029279.9052403504, "currency" : "EURO" }, { "account_type" : "Investment", "account_balance" : 5557778.601756326, "currency" : "YUAN" }, { "account_type" : "Investment", "account_balance" : 4529905.238752328, "currency" : "PESO" }, { "account_type" : "401K", "account_balance" : 5900849.270542218, "currency" : "YUAN" }, { "account_type" : "Investment", "account_balance" : 9999480.13929081, "currency" : "YEN" }, { "account_type" : "Savings", "account_balance" : 8280434.977165401, "currency" : "EURO" } ] }
{ "_id" : ObjectId("546961b4ca357ca95f30ae0c"), "first_name" : "EARL", "last_name" : "NAVARRO", "accounts" : [ { "account_type" : "401K", "account_balance" : 1530719.4163466224, "currency" : "USD" }, { "account_type" : "Investment", "account_balance" : 9999090.143345077, "currency" : "POUNDS STERLING" }, { "account_type" : "401K", "account_balance" : 1914183.7856416223, "currency" : "YUAN" }, { "account_type" : "Savings", "account_balance" : 901865.6920296131, "currency" : "YUAN" }, { "account_type" : "401K", "account_balance" : 6268518.440819243, "currency" : "YUAN" }, { "account_type" : "401K", "account_balance" : 5376494.119598793, "currency" : "PESO" }, { "account_type" : "Savings", "account_balance" : 8474789.101189336, "currency" : "YUAN" }, { "account_type" : "Savings", "account_balance" : 5855245.647297914, "currency" : "EURO" }, { "account_type" : "Investment", "account_balance" : 1604485.2395725362, "currency" : "EURO" } ] }

```

## Conclusion

In this section we covered how to get mongodb ip and running your machine as well as how to go about doing simple queries from the interactive shell. 

MongoDB has a unique querying interface where a *projection* object is specified which can contain operators and selects which fields should be used to match documents in the collection. We can chain these queries with the **sort()** function to return our documents in any particular order we need. In addition we can specify a second object to specify only the fields in the document should be included in the results.

Module 3 will dive into the Node.js and the C# language drivers and demonstrate how we can create and update Person entities in the database.
