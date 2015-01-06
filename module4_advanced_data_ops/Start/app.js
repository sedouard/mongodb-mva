var MongoClient = require('mongodb').MongoClient
  , assert = require('assert');
var ObjectID = require('mongodb').ObjectID;
// Connection URL
var url = 'mongodb://127.0.0.1:27017/test';
// Use connect method to connect to the Server
MongoClient.connect(url, function(err, db) {

  assert.equal(null, err);
  console.log("Connected correctly to server");
  var crimes = db.collection('crimes');
  

  //Question: What day did most crimes occur in chicago from September 2001 to present?

  //question what time of day do most crimes occur?


  //Question: what is the most common types of crimes commit in chicago since 2001?

  db.close();


});