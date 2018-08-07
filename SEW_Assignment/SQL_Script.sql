/*


drop table OrderDetails 
drop table OrderMasters
drop table Discounts
drop table Carts
drop table Customers
drop table Items
*/

create table Items
(
	ItemID				BIGINT PRIMARY KEY IDENTITY,
	ItemName			VARCHAR(200),
	ItemDescription		VARCHAR(4000),
	ItemQty				INT,
	ItemWeight			DECIMAL,
	BasePrice			DECIMAL
)

create table Discounts
(
	DiscountID				BIGINT PRIMARY KEY IDENTITY,
	DiscountDescription		VARCHAR(200),
	ItemID					BIGINT REFERENCES Items(ItemID), 
	-- we can extend the discount functionality 
	-- if Item id is blank then Discount percentage will be applied on Total invoice value	
	FreeItemID BIGINT REFERENCES Items(ItemID),  -- providing free item for buying another item, qulaify only meet the buy qty
	EffectiveDateFrom		DATETIME,
	EffectiveDateTo			DATETIME,
	DiscountPercentage		DECIMAL,
	BuyQty					INT,
	FreeQty					INT
)

create table Customers
(
	CustID		BIGINT IDENTITY PRIMARY KEY,
	CustFName	VARCHAR(200) NOT NULL,
	CustLName	VARCHAR(200),
	CustEmailID VARCHAR(200)
	/*
		we can add more fields like address , .....etc
	*/
)



create table Carts
(
	CartID			BIGINT PRIMARY KEY IDENTITY,
	CustID			BIGINT REFERENCES Customers(CustID),
	ItemID			BIGINT REFERENCES Items(ItemID),
	Qty				INT	,
	Discount		DECIMAL,
	Price			DECIMAL,
	TotalCost		DECIMAL,
	DiscountName	VARCHAR(100)
	
)

create table OrderMasters -- Order Master data would be used as cash register
(
	OrderID			BIGINT PRIMARY KEY IDENTITY,
	CustID			BIGINT REFERENCES Customers(CustID),
	TotalDiscount	DECIMAL,
	TotalCost		DECIMAL

	/*
		we can add more fields like payment detail ,tax .....etc
	*/
)



create table OrderDetails 
(

	DetailID		BIGINT PRIMARY KEY IDENTITY,
	OrderMasterID	BIGINT REFERENCES OrderMasters(OrderID),	
	ItemID			BIGINT REFERENCES Items(ItemID),
	Qty				INT	,
	Discount		DECIMAL,
	Price			DECIMAL,
	TotalCost		DECIMAL,
	DiscountName	VARCHAR(100)
	
)