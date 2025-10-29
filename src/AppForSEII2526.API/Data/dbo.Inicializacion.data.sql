INSERT INTO [dbo].[Model] ([Id], [Name]) VALUES (N'1', N'Corrola')
INSERT INTO [dbo].[Model] ([Id], [Name]) VALUES (N'10', N'Leaf e+')
INSERT INTO [dbo].[Model] ([Id], [Name]) VALUES (N'2', N'Civic')
INSERT INTO [dbo].[Model] ([Id], [Name]) VALUES (N'3', N'Escape')
INSERT INTO [dbo].[Model] ([Id], [Name]) VALUES (N'4', N'Model 3')
INSERT INTO [dbo].[Model] ([Id], [Name]) VALUES (N'5', N'Selverado')
INSERT INTO [dbo].[Model] ([Id], [Name]) VALUES (N'6', N'Wrangler')
INSERT INTO [dbo].[Model] ([Id], [Name]) VALUES (N'7', N'Serie 3')
INSERT INTO [dbo].[Model] ([Id], [Name]) VALUES (N'8', N'Golf GTI')
INSERT INTO [dbo].[Model] ([Id], [Name]) VALUES (N'9', N'Tucson Hybrid')

SET IDENTITY_INSERT [dbo].[Car] ON
INSERT INTO [dbo].[Car] ([Id], [CarClass], [Color], [Description], [EngDisplacement], [FuelType], [MaintenanceTypes], [Manufacturer], [PurchasingPrice], [QuantityForPurchasing], [QuantityForRenting], [RentingPrice], [RimSize], [ModelId]) VALUES (5, N'Compacto', N'Blanco', N'Toyota Corolla 1.8L Sedan', N'1.8', N'Gasolina', N'Estandar', N'Toyota', CAST(22000.00 AS Decimal(10, 2)), 5, 2, CAST(75.00 AS Decimal(10, 2)), 16, N'1')
INSERT INTO [dbo].[Car] ([Id], [CarClass], [Color], [Description], [EngDisplacement], [FuelType], [MaintenanceTypes], [Manufacturer], [PurchasingPrice], [QuantityForPurchasing], [QuantityForRenting], [RentingPrice], [RimSize], [ModelId]) VALUES (6, N'Deportivo', N'Rojo', N'Honda Civic Type R', N'2.0', N'Gasolina', N'Premium', N'Honda', CAST(38000.00 AS Decimal(10, 2)), 3, 1, CAST(120.00 AS Decimal(10, 2)), 19, N'2')
INSERT INTO [dbo].[Car] ([Id], [CarClass], [Color], [Description], [EngDisplacement], [FuelType], [MaintenanceTypes], [Manufacturer], [PurchasingPrice], [QuantityForPurchasing], [QuantityForRenting], [RentingPrice], [RimSize], [ModelId]) VALUES (7, N'SUV', N'Gris', N'Ford Escape Titanium', N'2.5', N'Hibrido', N'Estandar', N'Ford', CAST(32000.00 AS Decimal(10, 2)), 4, 2, CAST(95.00 AS Decimal(10, 2)), 17, N'3')
INSERT INTO [dbo].[Car] ([Id], [CarClass], [Color], [Description], [EngDisplacement], [FuelType], [MaintenanceTypes], [Manufacturer], [PurchasingPrice], [QuantityForPurchasing], [QuantityForRenting], [RentingPrice], [RimSize], [ModelId]) VALUES (9, N'Electrico', N'Azul', N'Tesla Model 3 Standard Range', N'-', N'Electrico', N'Bajo', N'Tesla', CAST(40000.00 AS Decimal(10, 2)), 6, 3, CAST(110.00 AS Decimal(10, 2)), 18, N'4')
INSERT INTO [dbo].[Car] ([Id], [CarClass], [Color], [Description], [EngDisplacement], [FuelType], [MaintenanceTypes], [Manufacturer], [PurchasingPrice], [QuantityForPurchasing], [QuantityForRenting], [RentingPrice], [RimSize], [ModelId]) VALUES (10, N'Pick Up', N'Negro', N'Chevrolet Silverado 1500', N'5.3', N'Gasolina', N'Pesado', N'Chevrolet', CAST(45000.00 AS Decimal(10, 2)), 2, 1, CAST(130.00 AS Decimal(10, 2)), 20, N'5')
INSERT INTO [dbo].[Car] ([Id], [CarClass], [Color], [Description], [EngDisplacement], [FuelType], [MaintenanceTypes], [Manufacturer], [PurchasingPrice], [QuantityForPurchasing], [QuantityForRenting], [RentingPrice], [RimSize], [ModelId]) VALUES (11, N'SUV', N'Verde', N'Jeep Wrangler Rubicon', N'3.6', N'Gasolina', N'Pesado', N'Jeep', CAST(55000.00 AS Decimal(10, 2)), 3, 1, CAST(150.00 AS Decimal(10, 2)), 18, N'6')
INSERT INTO [dbo].[Car] ([Id], [CarClass], [Color], [Description], [EngDisplacement], [FuelType], [MaintenanceTypes], [Manufacturer], [PurchasingPrice], [QuantityForPurchasing], [QuantityForRenting], [RentingPrice], [RimSize], [ModelId]) VALUES (12, N'Sedan', N'Plata', N'BMW Serie 3 320i', N'2.0', N'Gasolina', N'Premium', N'BMW', CAST(47000.00 AS Decimal(10, 2)), 4, 2, CAST(140.00 AS Decimal(10, 2)), 18, N'7')
INSERT INTO [dbo].[Car] ([Id], [CarClass], [Color], [Description], [EngDisplacement], [FuelType], [MaintenanceTypes], [Manufacturer], [PurchasingPrice], [QuantityForPurchasing], [QuantityForRenting], [RentingPrice], [RimSize], [ModelId]) VALUES (13, N'Hatchback', N'Naranja', N'Volkswagen Golf GTI', N'2.0', N'Gasolina', N'Estandar', N'Volkswagen', CAST(30000.00 AS Decimal(10, 2)), 5, 3, CAST(100.00 AS Decimal(10, 2)), 17, N'8')
INSERT INTO [dbo].[Car] ([Id], [CarClass], [Color], [Description], [EngDisplacement], [FuelType], [MaintenanceTypes], [Manufacturer], [PurchasingPrice], [QuantityForPurchasing], [QuantityForRenting], [RentingPrice], [RimSize], [ModelId]) VALUES (14, N'SUV', N'Blanco', N'Hyundai Tucson Hybrid', N'1.6', N'Hibrido', N'Estandar', N'Hyundai', CAST(31000.00 AS Decimal(10, 2)), 4, 2, CAST(95.00 AS Decimal(10, 2)), 17, N'9')
INSERT INTO [dbo].[Car] ([Id], [CarClass], [Color], [Description], [EngDisplacement], [FuelType], [MaintenanceTypes], [Manufacturer], [PurchasingPrice], [QuantityForPurchasing], [QuantityForRenting], [RentingPrice], [RimSize], [ModelId]) VALUES (15, N'Electrico', N'Plata', N'Nissan Leaf e+', N'-', N'Electrico', N'Bajo', N'Nissan', CAST(37000.00 AS Decimal(10, 2)), 5, 3, CAST(105.00 AS Decimal(10, 2)), 16, N'10')
SET IDENTITY_INSERT [dbo].[Car] OFF

INSERT INTO [dbo].[AspNetUsers] ([Id], [ClientAddress], [ClientPhoneNumber], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'U1', N'Calle Sol 123, Madrid', N'600000001', N'Carlos', N'Lopez', N'carlitos_l', N'CARLITOS_L', N'carlos@gmail.com', N'carlos@gmail.com', 1, NULL, NULL, NULL, NULL, 0, 0, NULL, 0, 0)
INSERT INTO [dbo].[AspNetUsers] ([Id], [ClientAddress], [ClientPhoneNumber], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'U2', N'Av. del Mar 45, Valencia', N'600000002', N'Ana', N'Matinez', N'anita_m', N'ANITA_M', N'ana@gmail.com', N'ana@gmail.com', 1, NULL, NULL, NULL, NULL, 0, 0, NULL, 0, 0)
INSERT INTO [dbo].[AspNetUsers] ([Id], [ClientAddress], [ClientPhoneNumber], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'U3', N'Calle Mayor 12, Sevilla', N'600000003', N'Lucia', N'Garcia', N'luci_g', N'LUCI_G', N'lucia@gmail.com', N'lucia@gmail.com', 1, NULL, NULL, NULL, NULL, 0, 0, NULL, 0, 0)
INSERT INTO [dbo].[AspNetUsers] ([Id], [ClientAddress], [ClientPhoneNumber], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'U4', N'Av. Aragon 87, Zaragoza', N'600000004', N'David', N'Perez', N'davidp', N'DAVIDP', N'david@gmail.com', N'david@gmail.com', 1, NULL, NULL, NULL, NULL, 0, 0, NULL, 0, 0)
INSERT INTO [dbo].[AspNetUsers] ([Id], [ClientAddress], [ClientPhoneNumber], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'U5', N'Calle Real 7, Bilbao', N'600000005', N'Marta', N'Santos', N'marta_s', N'MARTA_S', N'marta@gmail.com', N'marta@gmail.com', 1, NULL, NULL, NULL, NULL, 0, 0, NULL, 0, 0)
INSERT INTO [dbo].[AspNetUsers] ([Id], [ClientAddress], [ClientPhoneNumber], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'U6', N'Av. de la Guardia Civil', N'600000006', N'Alejandro', N'Patino', N'alex_pati', N'ALEX_PATI', N'alex@gmail.com', N'alex@gmail.com', 1, NULL, NULL, NULL, NULL, 0, 0, NULL, 0, 0)



SET IDENTITY_INSERT [dbo].[Maintenance] ON
INSERT INTO [dbo].[Maintenance] ([Id], [Name], [NumberOfDays], [Price]) VALUES (1, N'Cambio aceite', 1, CAST(100.00 AS Decimal(10, 2)))
INSERT INTO [dbo].[Maintenance] ([Id], [Name], [NumberOfDays], [Price]) VALUES (2, N'Revision frenos', 2, CAST(150.00 AS Decimal(10, 2)))
INSERT INTO [dbo].[Maintenance] ([Id], [Name], [NumberOfDays], [Price]) VALUES (3, N'Sustitucion bateria', 1, CAST(120.00 AS Decimal(10, 2)))
INSERT INTO [dbo].[Maintenance] ([Id], [Name], [NumberOfDays], [Price]) VALUES (4, N'Alineacion ruedas', 1, CAST(80.00 AS Decimal(10, 2)))
INSERT INTO [dbo].[Maintenance] ([Id], [Name], [NumberOfDays], [Price]) VALUES (5, N'Cambio filtros', 1, CAST(90.00 AS Decimal(10, 2)))
INSERT INTO [dbo].[Maintenance] ([Id], [Name], [NumberOfDays], [Price]) VALUES (6, N'Revision completa', 3, CAST(250.00 AS Decimal(10, 2)))
SET IDENTITY_INSERT [dbo].[Maintenance] OFF

SET IDENTITY_INSERT [dbo].[MaintenanceType] ON
INSERT INTO [dbo].[MaintenanceType] ([id], [Type], [MaintenanceId]) VALUES (2, N'Motor', 1)
INSERT INTO [dbo].[MaintenanceType] ([id], [Type], [MaintenanceId]) VALUES (3, N'Frenos', 2)
INSERT INTO [dbo].[MaintenanceType] ([id], [Type], [MaintenanceId]) VALUES (4, N'Electricidad', 3)
INSERT INTO [dbo].[MaintenanceType] ([id], [Type], [MaintenanceId]) VALUES (5, N'Ruedas', 4)
INSERT INTO [dbo].[MaintenanceType] ([id], [Type], [MaintenanceId]) VALUES (6, N'Filtros', 5)
INSERT INTO [dbo].[MaintenanceType] ([id], [Type], [MaintenanceId]) VALUES (7, N'General', 6)
SET IDENTITY_INSERT [dbo].[MaintenanceType] OFF


SET IDENTITY_INSERT [dbo].[Rental] ON
INSERT INTO [dbo].[Rental] ([Id], [DeliveryCarDealer], [EndDate], [RentingDate], [StartDate], [TotalPrice], [PaymentMethod], [ClientId]) VALUES (1, N'Madrid Center', N'2025-10-20 00:00:00', N'2025-10-15 00:00:00', N'2025-10-16 00:00:00', CAST(300.00 AS Decimal(10, 2)), N'Tarjeta', N'U1')
INSERT INTO [dbo].[Rental] ([Id], [DeliveryCarDealer], [EndDate], [RentingDate], [StartDate], [TotalPrice], [PaymentMethod], [ClientId]) VALUES (2, N'Valencia Autos', N'2025-11-05 00:00:00', N'2025-11-01 00:00:00', N'2025-11-02 00:00:00', CAST(250.00 AS Decimal(10, 2)), N'Transferencia', N'U2')
INSERT INTO [dbo].[Rental] ([Id], [DeliveryCarDealer], [EndDate], [RentingDate], [StartDate], [TotalPrice], [PaymentMethod], [ClientId]) VALUES (3, N'Sevilla Rent', N'2025-09-28 00:00:00', N'2025-08-20 00:00:00', N'2025-09-21 00:00:00', CAST(400.00 AS Decimal(10, 2)), N'Efectivo', N'U3')
INSERT INTO [dbo].[Rental] ([Id], [DeliveryCarDealer], [EndDate], [RentingDate], [StartDate], [TotalPrice], [PaymentMethod], [ClientId]) VALUES (4, N'Zaragoza Cars', N'2025-10-10 00:00:00', N'2025-09-20 00:00:00', N'2025-09-21 00:00:00', CAST(350.00 AS Decimal(10, 2)), N'Tarjeta', N'U4')
INSERT INTO [dbo].[Rental] ([Id], [DeliveryCarDealer], [EndDate], [RentingDate], [StartDate], [TotalPrice], [PaymentMethod], [ClientId]) VALUES (5, N'Bilbao Drive', N'2025-12-01 00:00:00', N'2025-11-25 00:00:00', N'2025-11-26 00:00:00', CAST(280.00 AS Decimal(10, 2)), N'Tarjeta', N'U5')
SET IDENTITY_INSERT [dbo].[Rental] OFF

INSERT INTO [dbo].[RentalItem] ([CarId], [RentalId], [Quantity]) VALUES (5, 1, 1)
INSERT INTO [dbo].[RentalItem] ([CarId], [RentalId], [Quantity]) VALUES (6, 2, 1)
INSERT INTO [dbo].[RentalItem] ([CarId], [RentalId], [Quantity]) VALUES (7, 3, 1)
INSERT INTO [dbo].[RentalItem] ([CarId], [RentalId], [Quantity]) VALUES (9, 4, 1)
INSERT INTO [dbo].[RentalItem] ([CarId], [RentalId], [Quantity]) VALUES (10, 5, 1)


SET IDENTITY_INSERT [dbo].[Purchase] ON
INSERT INTO [dbo].[Purchase] ([Id], [PurchasingDate], [PurchasingPrice], [DriverType], [DeliveryCarDealer], [PaymentMethod], [Country], [ClientId]) VALUES (1, N'2025-08-10 00:00:00', CAST(35000.00 AS Decimal(10, 2)), N'Particular', N'Madrid Center', N'Tarjeta', N'España', N'U1')
INSERT INTO [dbo].[Purchase] ([Id], [PurchasingDate], [PurchasingPrice], [DriverType], [DeliveryCarDealer], [PaymentMethod], [Country], [ClientId]) VALUES (2, N'2025-09-25 00:00:00', CAST(20000.00 AS Decimal(10, 2)), N'Empresa', N'Valencia Autos', N'Transferencia', N'España', N'U2')
INSERT INTO [dbo].[Purchase] ([Id], [PurchasingDate], [PurchasingPrice], [DriverType], [DeliveryCarDealer], [PaymentMethod], [Country], [ClientId]) VALUES (3, N'2025-07-12 00:00:00', CAST(25000.00 AS Decimal(10, 2)), N'Particular', N'Sevilla Rent', N'Efectivo', N'España', N'U3')
INSERT INTO [dbo].[Purchase] ([Id], [PurchasingDate], [PurchasingPrice], [DriverType], [DeliveryCarDealer], [PaymentMethod], [Country], [ClientId]) VALUES (4, N'2025-10-01 00:00:00', CAST(22000.00 AS Decimal(10, 2)), N'Particular', N'Zaragoza Cars', N'Tarjeta', N'España', N'U4')
INSERT INTO [dbo].[Purchase] ([Id], [PurchasingDate], [PurchasingPrice], [DriverType], [DeliveryCarDealer], [PaymentMethod], [Country], [ClientId]) VALUES (5, N'2025-06-22 00:00:00', CAST(30000.00 AS Decimal(10, 2)), N'Empresa', N'Bilbao Drive', N'Transferencia', N'España', N'U5')
SET IDENTITY_INSERT [dbo].[Purchase] OFF

INSERT INTO [dbo].[PurchaseItem] ([CarId], [PurchaseId], [Quantity]) VALUES (5, 1, 1)
INSERT INTO [dbo].[PurchaseItem] ([CarId], [PurchaseId], [Quantity]) VALUES (6, 2, 1)
INSERT INTO [dbo].[PurchaseItem] ([CarId], [PurchaseId], [Quantity]) VALUES (7, 3, 1)
INSERT INTO [dbo].[PurchaseItem] ([CarId], [PurchaseId], [Quantity]) VALUES (9, 4, 1)
INSERT INTO [dbo].[PurchaseItem] ([CarId], [PurchaseId], [Quantity]) VALUES (10, 5, 1)


SET IDENTITY_INSERT [dbo].[Review] ON
INSERT INTO [dbo].[Review] ([Id], [Created], [Country], [DriverType], [ClientId]) VALUES (1, N'2025-10-10 00:00:00', N'España', N'Particular', N'U1')
INSERT INTO [dbo].[Review] ([Id], [Created], [Country], [DriverType], [ClientId]) VALUES (2, N'2025-09-20 00:00:00', N'España', N'Particular', N'U2')
INSERT INTO [dbo].[Review] ([Id], [Created], [Country], [DriverType], [ClientId]) VALUES (3, N'2025-08-12 00:00:00', N'España', N'Empresa', N'U3')
INSERT INTO [dbo].[Review] ([Id], [Created], [Country], [DriverType], [ClientId]) VALUES (4, N'2025-07-05 00:00:00', N'España', N'Particular', N'U4')
INSERT INTO [dbo].[Review] ([Id], [Created], [Country], [DriverType], [ClientId]) VALUES (5, N'2025-06-25 00:00:00', N'España', N'Particular', N'U6')
SET IDENTITY_INSERT [dbo].[Review] OFF


INSERT INTO [dbo].[ReviewItem] ([CarId], [ReviewId], [Description], [Rating]) VALUES (5, 1, N'Excelente coche eléctrico, muy cómodo.', 5)
INSERT INTO [dbo].[ReviewItem] ([CarId], [ReviewId], [Description], [Rating]) VALUES (6, 2, N'Buen rendimiento y consumo ajustado.', 4)
INSERT INTO [dbo].[ReviewItem] ([CarId], [ReviewId], [Description], [Rating]) VALUES (7, 3, N'Perfecto para ciudad, motor silencioso.', 5)
INSERT INTO [dbo].[ReviewItem] ([CarId], [ReviewId], [Description], [Rating]) VALUES (9, 4, N'Conducción suave y ágil.', 4)
INSERT INTO [dbo].[ReviewItem] ([CarId], [ReviewId], [Description], [Rating]) VALUES (10, 5, N'Espacioso y potente, ideal para familia.', 5)


SET IDENTITY_INSERT [dbo].[Booking] ON
INSERT INTO [dbo].[Booking] ([Id], [Date], [PaymentMethod], [ClientId]) VALUES (1, N'2025-10-18 00:00:00', N'Tarjeta', N'U1')
INSERT INTO [dbo].[Booking] ([Id], [Date], [PaymentMethod], [ClientId]) VALUES (2, N'2025-10-19 00:00:00', N'Transferencia', N'U2')
INSERT INTO [dbo].[Booking] ([Id], [Date], [PaymentMethod], [ClientId]) VALUES (3, N'2025-10-20 00:00:00', N'Efectivo', N'U3')
INSERT INTO [dbo].[Booking] ([Id], [Date], [PaymentMethod], [ClientId]) VALUES (4, N'2025-10-21 00:00:00', N'Tarjeta', N'U4')
INSERT INTO [dbo].[Booking] ([Id], [Date], [PaymentMethod], [ClientId]) VALUES (5, N'2025-10-22 00:00:00', N'Efectivo', N'U5')
SET IDENTITY_INSERT [dbo].[Booking] OFF


INSERT INTO [dbo].[BookingItem] ([BookingId], [MaintenanceID], [Comment], [MantId]) VALUES (1, 1, N'NULLCambio de aceite anual', 1)
INSERT INTO [dbo].[BookingItem] ([BookingId], [MaintenanceID], [Comment], [MantId]) VALUES (2, 2, N'Revisión de frenos preventiva', 2)
INSERT INTO [dbo].[BookingItem] ([BookingId], [MaintenanceID], [Comment], [MantId]) VALUES (3, 3, N'Sustitución de batería defectuosa', 3)
INSERT INTO [dbo].[BookingItem] ([BookingId], [MaintenanceID], [Comment], [MantId]) VALUES (4, 4, N'Alineación antes de viaje', 4)
INSERT INTO [dbo].[BookingItem] ([BookingId], [MaintenanceID], [Comment], [MantId]) VALUES (5, 5, N'Cambio de filtro de aire', 5)

