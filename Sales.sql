--2. ���� ������� �������� ������� (����� ����): Sales: salesid, productid, datetime, customerid. 
--�� ����� ������, ����� ����� �������� ������� ��������� � ��� � �������. 
--�������� ������, ������� ������� ������� � ���������� �������, ����� �� ��� ������ �������� �������. 

create table Sales (
	salesid bigint Identity(1,1),
	productid bigint , 
	datetime datetime , 
	customerid bigint
)
ALTER TABLE Sales
ADD CONSTRAINT PK_Sales_salesid PRIMARY KEY  CLUSTERED (salesid)

insert into Sales(productid,datetime,customerid) values 
(1, '1970-01-01 23:59:00',1),--������ ������� 1 ������
(2, '1970-01-02 23:59:00',1),
(1, '1970-01-01 23:59:00',2),--������ ������� 1 ������
(2, '1970-01-02 23:59:00',2),
(2, '1970-01-01 23:59:00',3),--������ ������� 2 ������
(1, '1970-01-02 23:59:00',3),
(2, '1970-01-01 23:59:00',4),--������ ������� 2 ������
(1, '1970-01-02 23:59:00',4),
(3, '1970-01-02 23:59:00',5),--������ ������� 3 ������
(4, '1970-01-02 23:59:00',6)--������ ������� 4 ������
--��������� ��������� :
-- 1:2
--2:2
--3:1
--4:1
--������ �������
select productid, count(*) from
	(select customerid, min(datetime) as mindt from Sales group by customerid) as firstPay
		inner join sales  as s
		on firstPay.customerid = s.customerid and firstPay.mindt = s.datetime  
group by productid

-- ��� ����� ��� ���
select productid, count(*) from 
(select productid,  ROW_NUMBER() over (PARTITION BY customerid order by datetime asc ) as rowNum from Sales
) as pays 
where rowNum = 1
group by productid

--����� � ��� �������, ��������� ������� �� �������� �� ��������� ������� ������� � ��������� ���������.
--������ �����, ������ ������� �������. 