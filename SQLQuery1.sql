CREATE DATABASE hmdb
USE hmdb


-- Create Admin Table
CREATE TABLE Admin (
  admin_id INT PRIMARY KEY IDENTITY(1,1),
  username VARCHAR(255),
  email VARCHAR(255),
  password VARCHAR(255),
  first_name VARCHAR(255),
  last_name VARCHAR(255),
  con_pass varchar(255),
  image_data VARCHAR(255)
);

INSERT INTO Admin
VALUES('admin', 'admin@gmail.com', 'adminadmin', 'Sabbir', 'Hossain', 'adminadmin','')
SELECT * FROM Admin


-- Create Customer Info Table
CREATE TABLE CustomerInfo (
  customer_id INT PRIMARY KEY IDENTITY(1,1),
  username VARCHAR(255),
  email VARCHAR(255),
  password VARCHAR(255),
  first_name VARCHAR(255),
  last_name VARCHAR(255),
  address VARCHAR(255),
  city VARCHAR(255),
  state VARCHAR(255),
  country VARCHAR(255),
  phone_number VARCHAR(255),
  con_pass varchar(255),
  image_data VARCHAR(255)
);


-- Create Registered Table
CREATE TABLE Registered (
  registration_id INT PRIMARY KEY IDENTITY(1,1),
  customer_id INT,
  registration_date DATE,
  FOREIGN KEY (customer_id) REFERENCES CustomerInfo(customer_id)
);

-- Create Customer Login Table
CREATE TABLE CustomerLogin (
  login_id INT PRIMARY KEY IDENTITY(1,1),
  customer_id INT,
  email VARCHAR(255),
  password VARCHAR(255),
  date_time smalldatetime
  FOREIGN KEY (customer_id) REFERENCES CustomerInfo(customer_id)
);


-- Create Admin Login Table
CREATE TABLE AdminLogin (
  login_id INT PRIMARY KEY IDENTITY(1,1),
  admin_id INT,
  email VARCHAR(255),
  password VARCHAR(255),
  date_time smalldatetime
  FOREIGN KEY (admin_id) REFERENCES Admin(admin_id)
);



-- Create Products Table
CREATE TABLE Products (
  product_id INT PRIMARY KEY,
  product_name VARCHAR(255),
  description VARCHAR(MAX),
  price DECIMAL(10,2),
  availability INT,
  image_data VARCHAR(255),
  Category VARCHAR(255)
);

-- Create Services Table
CREATE TABLE Services (
  service_id INT PRIMARY KEY,
  service_name VARCHAR(255),
  description VARCHAR(MAX),
  price DECIMAL(10,2),
  availability INT
);
CREATE TABLE ServiceRequested (
  req_id INT IDENTITY(1,1) PRIMARY KEY,
  service_id INT FOREIGN KEY REFERENCES Services(service_id),
  customer_id INT,
  req_date date,
  req_time time,
  req_address VARCHAR(MAX),
  req_district VARCHAR(100),
  req_status VARCHAR(50),
  FOREIGN KEY (customer_id) REFERENCES CustomerInfo(customer_id)
);

-- Create Wishlist Table
CREATE TABLE Wishlist (
  wishlist_id INT PRIMARY KEY IDENTITY(1,1),
  customer_id INT,
  product_id INT,
  FOREIGN KEY (customer_id) REFERENCES CustomerInfo(customer_id),
  FOREIGN KEY (product_id) REFERENCES Products(product_id)
);
CREATE TABLE WishlistRecord (
  wr int IDENTITY(1,1) PRIMARY KEY,
  wishlist_id INT,
  product_id INT,
  FOREIGN KEY (wishlist_id) REFERENCES Wishlist(wishlist_id),
  FOREIGN KEY (product_id) REFERENCES Products(product_id)
);


-- Create Cart Table
CREATE TABLE Cart (
  cart_id INT PRIMARY KEY IDENTITY(1,1),
  customer_id INT,
  price DECIMAL(10,2),
  FOREIGN KEY (customer_id) REFERENCES CustomerInfo(customer_id),
);

CREATE TABLE CartRecord (
  cr int identity(1,1) PRIMARY KEY,
  cart_id INT,
  product_id INT,
  quantity INT,
  price decimal(10, 2),
  status VARCHAR(50) NOT NULL,
  FOREIGN KEY (cart_id) REFERENCES Cart(cart_id),
  FOREIGN KEY (product_id) REFERENCES Products(product_id)
);
 SELECT * FROM CustomerInfo
 SELECT * FROM Checkout
 SELECT * FROM PaymentMethod
 SELECT * FROM ShippingAddress
 SELECT * FROM CartRecord
 SELECT * FROM Cart
 SELECT * FROM Orders
 SELECT * FROM Admin


-- Create Checkout Table
CREATE TABLE Checkout (
  checkout_id INT PRIMARY KEY IDENTITY(1,1),
  cart_id INT,
  customer_id INT,
  checkout_date DATE,
  FOREIGN KEY (cart_id) REFERENCES Cart(cart_id),
  FOREIGN KEY (customer_id) REFERENCES CustomerInfo(customer_id)
);


-- Create Buy History Table
CREATE TABLE BuyHistory (
  history_id INT PRIMARY KEY IDENTITY(1,1),
  customer_id INT,
  quantity INT,
  purchase_date DATE,
  FOREIGN KEY (customer_id) REFERENCES CustomerInfo(customer_id),
);
CREATE TABLE BuyRecord (
  br int IDENTITY(1,1) PRIMARY KEY,
  history_id INT,
  product_id INT,
  service_id INT,
  FOREIGN KEY (history_id) REFERENCES BuyHistory(history_id),
  FOREIGN KEY (product_id) REFERENCES Products(product_id),
  FOREIGN KEY (service_id) REFERENCES Services(service_id)
);
 


-- Create Order Table
CREATE TABLE Orders (
  order_id INT PRIMARY KEY IDENTITY(1,1),
  customer_id INT,
  order_status VARCHAR(255),
  orderdate datetime,
  feedback VARCHAR(MAX),
  cart_id int,
  FOREIGN KEY (customer_id) REFERENCES CustomerInfo(customer_id),
  FOREIGN KEY (cart_id) REFERENCES Cart(cart_id)
);
CREATE TABLE OrderRecord (
  or_id int IDENTITY(1,1) PRIMARY KEY,
  order_id INT,
  product_id INT,
  FOREIGN KEY (order_id) REFERENCES Orders(order_id),
  FOREIGN KEY (product_id) REFERENCES Products(product_id)
);

-- Create Reviews Table
CREATE TABLE Reviews (
  review_id INT PRIMARY KEY IDENTITY(1,1),
  customer_id INT,
  product_id INT,
  rating INT,
  comment VARCHAR(MAX),
  review_date DATE,
  FOREIGN KEY (customer_id) REFERENCES CustomerInfo(customer_id),
  FOREIGN KEY (product_id) REFERENCES Products(product_id)
);
CREATE TABLE Reports (
  report_id INT PRIMARY KEY IDENTITY(1,1),
  customer_email VARCHAR(MAX) NOT NULL,
  report_subject VARCHAR(MAX),
  report_details VARCHAR(MAX),
  report_date DATE,
  customer_name VARCHAR(MAX),
  customer_contact VARCHAR(MAX)
);



-- Create Shipping Address Table
CREATE TABLE ShippingAddress (
  address_id INT PRIMARY KEY IDENTITY(1,1),
  customer_id INT,
  address_line1 VARCHAR(255),
  address_line2 VARCHAR(255),
  city VARCHAR(255),
  state VARCHAR(255),
  postal_code VARCHAR(255),
  country VARCHAR(255),
  is_default BIT,
  FOREIGN KEY (customer_id) REFERENCES CustomerInfo(customer_id)
);

-- Create Payment Method Table
CREATE TABLE PaymentMethod (
  payment_method_id INT PRIMARY KEY IDENTITY(1,1),
  customer_id INT,
  card_number VARCHAR(255),
  cardholder_name VARCHAR(255),
  expiration_date VARCHAR(255),
  cvv VARCHAR(255),
  is_default BIT,
  FOREIGN KEY (customer_id) REFERENCES CustomerInfo(customer_id)
);



-- Change the column datatype


TRUNCATE TABLE CustomerInfo
SELECT * FROM CustomerInfo
SELECT * FROM Products