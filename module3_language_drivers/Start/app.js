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