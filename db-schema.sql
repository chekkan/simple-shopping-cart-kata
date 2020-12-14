-- auto-generated definition
create table Items
(
    orderId  int,
    quantity int,
    sku      varchar(20)
)
go

-- auto-generated definition
create table Orders
(
    orderId int identity,
    cusId   varchar(20) not null
)
go

create unique index Orders_orderId_uindex
    on Orders (orderId)
go

-- auto-generated definition
create table Products
(
    sku   varchar(20) not null,
    name  varchar(20) not null,
    price int     not null
)
go

create unique index Products_sku_uindex
    on Products (sku)
go

