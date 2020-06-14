UPDATE business
SET numcheckins = (SELECT count(checkinbusinessid) from checkin WHERE business.businessid = checkin.checkinbusinessid)

UPDATE business
SET reviewcount = (SELECT count(businessid) from review WHERE business.businessid = review.businessid)

UPDATE business
SET reviewrating = (SELECT SUM(stars) from review WHERE business.businessid = review.businessid) / business.reviewcount