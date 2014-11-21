# Module 1: Introduction to NoSQL Document Oriented Databases

--------
## Objectives

By the end of this module you will be able to:

- Understand the difference between Documented Oriented Databases, Structured Databases and SQL Databases
- How a Document Oriented Database is laid out

## Introduction

[Document Oriented Databases](http://en.wikipedia.org/wiki/Document-oriented_database) are a relatively new type of data store which organizes data radically different than your traditional SQL store. Rather than storing items in a variety of tables, document databases like [MongoDB](http://mongodb.org) store data in 'documents' which wholly describe an entire entity in your application.

This data organization scheme eliminates the need for most [joins](http://en.wikipedia.org/wiki/Join_(SQL)) between tables to collect information associated with an entity. In general, document oriented databases are best suited for data need which involve a large amount of one:one and one:many relationships.

Further, storage databases like MongoDB don't have strict entity relationship structures like SQL introduces. Most notably, there is no such thing as [foriegn keys](http://en.wikipedia.org/wiki/Foreign_key) or build-in columns which are used to identify keys in other tables. Traditionally this is a pain point when trying to make any large data organization changes to a SQL database.

## Tabluar Structured Data vs Document Structure


Lets look at an example of a bank, which keeps track of the following entities:

- **Person**
- **Account**

The basic relation here is that a **Person** can have many **Accounts** and therefore an **Account** has only one **Person**.

### Tabular Structure

Here's what this data may look like in a SQL database

**Persons:**


ID  | FIRST_NAME | LAST_NAME
------------- | ------------- | -------------
0  | Steven | Edouard
1 | Sam | Brightwood


**Accounts**

ID  | ACCOUNT_TYPE | ACCOUNT_BALANCE | CURRENCY | HOLDER (FK: Persons)
------------- | ------------- | ------------- | ------------- | -------------
0  | Investment | 80000.00 | USD | 0
1  | Savings | 70400.00 | USD | 0
2  | Checking | 4500.00 | USD | 0
3  | Checking | 4500.00 | YEN | 1
4  | Investment | 4500.00 | YEN | 1
5  | Savings | 4500.00 | YEN | 1

In the above data schema, the **HOLDER** field in the **Accounts** table is a **Foriegn Key** into the **Accounts** table. This is what creates the association between the two data entities. SQL actually requires that in order to create an Account, you must have a valid Foreign key that points to an existing person, which can cause some loss in flexibility.

With the schema above its easy to answer the question:

*Who is the account holder for account with ID = 3?*

Because we have a foreign key HOLDER for the account we can quickly and easily look up that the account holder is **Sam Brightwood**. This is because there is a **many:one** relation ships from Account entities and Person entities.

Now, how about the question:

*Which accounts does person Steven Edouard hold?*

To answer this question, it requires us to do a **join** which essentially is a set operation to find all of the rows in the **Accounts** table which match the Person ID = 0. This can become an expensive operation as the data in the tables grow toward larger and larger numbers.

### Document Oriented Structure

The same data above can be described in a document oriented database with a JSON-like syntax:

```json
# Person Collection (Person is the root entity)
[
{
	"ID": 0,
	"first_name": "Steven",
	"last_name": "Edouard",
	"accounts": [
		{
			"id": 0,
			"account_type": "Investment",
			"account_balance": "80000.00",
			"currency": "USD"
		},
		{
			"id": 1,
			"account_type": "Savings",
			"account_balance": "70400.00",
			"currency": "USD"
		},
		{
			"id": 2,
			"account_type": "Checking",
			"account_balance": "80000.00",
			"currency": "USD"
		}
	]
}
{
	"ID": 1,
	"first_name": "Sam",
	"last_name": "Brightwood",
	"accounts":[
		
		{
			"id": 3,
			"account_type": "Checking",
			"account_balance": "4500.00",
			"currency": "YEN"
		}
		{
			"id": 4,
			"account_type": "Investment",
			"account_balance": "4500.00",
			"currency": "YEN"
		},
		{

			"id": 5,
			"account_type": "Savings",
			"account_balance": "4500.00",
			"currency": "YEN"
		}
	]
}	

]

```
Now, let's answer the original two questions again:

*Who is the account holder for account with ID = 3?*

To answer this question we can just look up the account with the unique ID 3, and by finding that account, we automatically pull the associated user. This is just as fast as the tabular organization.

*Which accounts does person Steven Edouard hold?*

This question can be answered by looking for the person with ```first_name="Steven"``` and ```last_name="Edouard"``. Once we locate that person, the associated accounts are automatically found since they are associated in the database organization. This type of query proves to be much faster than the tabular approach.

Notice how this looks *very* different than the tabular representation. This is a very clever way of storing our data because it the relationship between Persons and Accounts is inherent in the structure of the data. Meaning, when we retrieve the Person, Steven Edouard, we also retrieve all of his bank accounts, eliminating the need for any expensive joins here.

This is the fundamental difference between NoSQL Document stores and SQL databases.


