--2. Есть таблица хранящая покупки (линии чека): Sales: salesid, productid, datetime, customerid. 
--Мы хотим понять, через какие продукты клиенты «попадают» к нам в магазин. 
--Напишите запрос, который выводит продукт и количество случаев, когда он был первой покупкой клиента. 

create table Sales (
	salesid bigint Identity(1,1),
	productid bigint , 
	datetime datetime , 
	customerid bigint
)
ALTER TABLE Sales
ADD CONSTRAINT PK_Sales_salesid PRIMARY KEY  CLUSTERED (salesid)

insert into Sales(productid,datetime,customerid) values 
(1, '1970-01-01 23:59:00',1),--первая покупка 1 товара
(2, '1970-01-02 23:59:00',1),
(1, '1970-01-01 23:59:00',2),--первая покупка 1 товара
(2, '1970-01-02 23:59:00',2),
(2, '1970-01-01 23:59:00',3),--первая покупка 2 товара
(1, '1970-01-02 23:59:00',3),
(2, '1970-01-01 23:59:00',4),--первая покупка 2 товара
(1, '1970-01-02 23:59:00',4),
(3, '1970-01-02 23:59:00',5),--первая покупка 3 товара
(4, '1970-01-02 23:59:00',6)--первая покупка 4 товара
--Ожидаемый результат :
-- 1:2
--2:2
--3:1
--4:1
--Чистый вариант
select productid, count(*) from
	(select customerid, min(datetime) as mindt from Sales group by customerid) as firstPay
		inner join sales  as s
		on firstPay.customerid = s.customerid and firstPay.mindt = s.datetime  
group by productid

-- Еще можно вот так
select productid, count(*) from 
(select productid,  ROW_NUMBER() over (PARTITION BY customerid order by datetime asc ) as rowNum from Sales
) as pays 
where rowNum = 1
group by productid

--Планы у них похожие, конкретно замерял бы скорость на настоящей большой таблице с реальными индексами.
--Скорее всего, первый вариант быстрее. 