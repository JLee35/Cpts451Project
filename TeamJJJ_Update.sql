UPDATE business
SET numcheckins = (SELECT sum(checkInAmount) from checkin WHERE business.businessid = checkin.checkinbusinessid);

UPDATE business
SET numcheckins =
	CASE
		WHEN 
			numcheckins IS NULL
		THEN
			0
		ELSE
			numcheckins
		END;

UPDATE business
SET reviewcount = (SELECT count(businessid) from review WHERE business.businessid = review.businessid);

UPDATE business
SET reviewrating = (SELECT SUM(stars) from review WHERE business.businessid = review.businessid) / business.reviewcount;