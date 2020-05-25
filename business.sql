
DROP TABLE IF EXISTS Business CASCADE;

CREATE TABLE Business (
	business_id char(22) PRIMARY KEY, 
	name varchar,
	state char(2),
	city varchar
);