
CREATE DATABASE FoodGo;
USE FoodGo;
SELECT DB_NAME() AS CurrentDatabase;

CREATE TABLE Users(UserId INT IDENTITY(1,1) PRIMARY KEY,Name NVARCHAR(100) NOT NULL,
Email NVARCHAR(150) NOT NULL UNIQUE,PasswordHash NVARCHAR(255) NOT NULL,Phone NVARCHAR(20) NULL);

INSERT INTO Users
    (Name, Email, PasswordHash, Phone)
VALUES
    ('Rahul', 'rahul@gmail.com', 'TEMP_HASH_123', '9876543210'),
    ('Priya', 'priya@gmail.com', 'TEMP_HASH_456', '9876543211');

    SELECT * FROM Users;

    CREATE TABLE Restaurants
      (RestaurantId INT IDENTITY(1,1) PRIMARY KEY,Name NVARCHAR(150) NOT NULL,
    Address NVARCHAR(250) NOT NULL,City NVARCHAR(100) NOT NULL,
    Rating DECIMAL(2,1) NULL,DeliveryTime NVARCHAR(50) NULL);

INSERT INTO Restaurants
    (Name, Address, City, Rating, DeliveryTime)
VALUES
    ('Paradise Biryani', 'Secunderabad', 'Hyderabad', 4.5, '30 mins'),
    ('Bawarchi', 'RTC X Roads', 'Hyderabad', 4.3, '35 mins'),
    ('Dominos', 'Kukatpally', 'Hyderabad', 4.2, '25 mins');

    SELECT * FROM Restaurants;

    CREATE TABLE Categories
(
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE
);

INSERT INTO Categories (Name)
VALUES
    ('Biryani'),
    ('Pizza'),
    ('Burgers'),
    ('South Indian'),
    ('Desserts'),
    ('Chinese');

    

     CREATE TABLE FoodItems
(
    FoodItemId INT IDENTITY(1,1) PRIMARY KEY,
    RestaurantId INT NOT NULL,
    CategoryId INT NOT NULL,
    Name NVARCHAR(150) NOT NULL,
    Description NVARCHAR(500) NULL,
    Price DECIMAL(10,2) NOT NULL,
    IsAvailable BIT NOT NULL DEFAULT 1,

    CONSTRAINT FK_FoodItems_Restaurants
        FOREIGN KEY (RestaurantId)
        REFERENCES Restaurants(RestaurantId),

    CONSTRAINT FK_FoodItems_Categories
        FOREIGN KEY (CategoryId)
        REFERENCES Categories(CategoryId)
);

INSERT INTO FoodItems
    (RestaurantId, CategoryId, Name, Description, Price, IsAvailable)
VALUES
    (1, 1, 'Chicken Biryani', 'Spicy chicken biryani', 250.00, 1),
    (1, 1, 'Mutton Biryani', 'Hyderabadi mutton biryani', 320.00, 1),
    (2, 1, 'Chicken Biryani', 'Special chicken biryani', 230.00, 1),
    (3, 2, 'Margherita Pizza', 'Classic cheese pizza', 299.00, 1),
    (3, 3, 'Veg Burger', 'Vegetable burger with cheese', 149.00, 1);

     SELECT * FROM Users;
     SELECT * FROM Restaurants;
     SELECT * FROM Categories;
    SELECT * FROM FoodItems;

    CREATE TABLE Addresses
(
    AddressId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    AddressLine NVARCHAR(250) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    State NVARCHAR(100) NOT NULL,
    Pincode NVARCHAR(10) NOT NULL,

    CONSTRAINT FK_Addresses_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId)
);
INSERT INTO Addresses
    (UserId, AddressLine, City, State, Pincode)
VALUES
    (1, '10 MG Road', 'Hyderabad', 'Telangana', '500001'),
    (1, '25 Hitech City', 'Hyderabad', 'Telangana', '500081'),
    (2, '15 Banjara Hills', 'Hyderabad', 'Telangana', '500034');

SELECT * FROM Addresses;

CREATE TABLE Cart
(
    CartId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL UNIQUE,

    CONSTRAINT FK_Cart_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId)
);

INSERT INTO Cart (UserId)
VALUES
    (1),
    (2);

    SELECT * FROM Cart;

    CREATE TABLE CartItems
(
    CartItemId INT IDENTITY(1,1) PRIMARY KEY,
    CartId INT NOT NULL,
    FoodItemId INT NOT NULL,
    Quantity INT NOT NULL,

    CONSTRAINT FK_CartItems_Cart
        FOREIGN KEY (CartId)
        REFERENCES Cart(CartId),

    CONSTRAINT FK_CartItems_FoodItems
        FOREIGN KEY (FoodItemId)
        REFERENCES FoodItems(FoodItemId)
);

INSERT INTO CartItems
    (CartId, FoodItemId, Quantity)
VALUES
    (1, 1, 2),
    (1, 2, 1);

   

    CREATE TABLE Orders
(
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    RestaurantId INT NOT NULL,
    AddressId INT NOT NULL,
    OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
    TotalAmount DECIMAL(10,2) NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Placed',

    CONSTRAINT FK_Orders_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_Orders_Restaurants
        FOREIGN KEY (RestaurantId)
        REFERENCES Restaurants(RestaurantId),

    CONSTRAINT FK_Orders_Addresses
        FOREIGN KEY (AddressId)
        REFERENCES Addresses(AddressId)
);

INSERT INTO Orders
    (UserId, RestaurantId, AddressId, TotalAmount, Status)
VALUES
    (1, 1, 1, 820.00, 'Placed');

   

    CREATE TABLE OrderItems
(
    OrderItemId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    FoodItemId INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL,

    CONSTRAINT FK_OrderItems_Orders
        FOREIGN KEY (OrderId)
        REFERENCES Orders(OrderId),

    CONSTRAINT FK_OrderItems_FoodItems
        FOREIGN KEY (FoodItemId)
        REFERENCES FoodItems(FoodItemId)
);

INSERT INTO OrderItems
    (OrderId, FoodItemId, Quantity, Price)
VALUES
    (1, 1, 2, 250.00),
    (1, 2, 1, 320.00);


     SELECT * FROM Users;
     SELECT * FROM Restaurants;
     SELECT * FROM Categories;
    SELECT * FROM FoodItems;

  SELECT * FROM Addresses;  
  SELECT * FROM Cart;
 SELECT * FROM CartItems;
 SELECT * FROM Orders;
SELECT * FROM OrderItems;


SELECT 
    FoodItemId,
    Name,
    Price,
    IsAvailable
FROM FoodItems;

 SELECT * FROM CartItems;

 UPDATE FoodItems
SET IsAvailable = 1
WHERE FoodItemId = 1;

INSERT INTO Cart (UserId)
VALUES (4);

SELECT *
FROM Cart;


