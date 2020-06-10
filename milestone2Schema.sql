/* CptS451, Team JJJ */

CREATE TABLE Business (
	businessID varchar PRIMARY KEY,
	avgScore real,
	city varchar,
	detailedInfo char(120),
	name varchar,
	numCheckins int,
	numReviews int,
	state varchar,
	stars real,
	zipcode int(5)

	/* TODO (Jared): Handle address attribute (multi-value) */
	/* TODO (Jared): Add relationships */
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
	isFanOf varchar, /* TODO (Jared): Is this correct? */
	isFriendsWith varchar, /* TODO (Jared): Is this correct? */
	numFans int,
	votes int

	/* TODO (Jared): Add relationships */
);

CREATE TABLE CheckIn (
	checkInDate date,
	checkInTime time

	/* TODO (Jared): Handled checked-in relationship */
);

CREATE TABLE Category (
	title varchar PRIMARY KEY,

	/* TODO (Jared): Handle categories relationship */
);

CREATE TABLE Review (
	reviewID varchar PRIMARY KEY,
	stars real,
	text char(120)

	/* TODO (Jared): Handle reviewed relationship */
);


