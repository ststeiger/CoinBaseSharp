
CREATE TABLE IF NOT EXISTS t_map_currency_rate
(
	cur_uid uuid NULL,
	cur_time timestamp without time zone NULL,
	cur_name char(3) NULL,
	cur_rate decimal(35, 15) NULL
);
