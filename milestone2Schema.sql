/* CptS451, Team JJJ */
DROP TABLE IF EXISTS Business CASCADE;
DROP TABLE IF EXISTS UserTable CASCADE;
DROP TABLE IF EXISTS CheckIn CASCADE;
DROP TABLE IF EXISTS Category CASCADE;
DROP TABLE IF EXISTS Review CASCADE;

CREATE TABLE Business (
	businessID varchar PRIMARY KEY,
	businessName varchar,
	address varchar,
	avgScore real,
	city varchar,
	detailedInfo char(120),
	numCheckins int,
	numReviews int,
	businessState varchar,
	stars real,
	openStatus int, /* 0 = closed, 1 = open */
	zip int

	/* TODO (Team): Add OpenTimes (should this be another entity?) */
	/* TODO (Team): Add category (I thought this was removed?) */
);

CREATE TABLE UserTable (
	userID varchar PRIMARY KEY,
	firstName varchar,
	lastName varchar,
	avgStars real,
	dateJoined Date,
	latitude real,
	longitude real,
	info char(120),
	isFanOf varchar, /* TODO (Team): Is this correct? */
	isFriendsWith varchar, /* TODO (Team): Is this correct? */
	numFans int,
	votes int,
	favorites varchar /* Does this need to point to a list of businessIDs? */
);

CREATE TABLE CheckIn (
	checkInDate date,
	checkInTime time,
	checkInBusinessID varchar NOT NULL,
	checkInUserId varchar NOT NULL,
	PRIMARY KEY (checkInUserID, checkInBusinessID, checkInDate, checkInTime),
	FOREIGN KEY (checkInBusinessID) REFERENCES Business(businessID),
	FOREIGN KEY (checkInUserID) REFERENCES UserTable(userID)

	/* TODO (Team): Double check relationships are correct. */
);

CREATE TABLE Category (
	title varchar PRIMARY KEY
	/* TODO (Team): Handle categories relationship */
);

CREATE TABLE Review (
	reviewID varchar PRIMARY KEY,
	userID varchar NOT NULL,
	businessID varchar NOT NULL,
	stars real,
	text char(120)
);
