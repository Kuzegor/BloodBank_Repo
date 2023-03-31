use BloodBank;
go

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.spDoctors_DeleteById
@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	delete from dbo.Doctors
	where Id = @Id;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spDoctors_GetAll
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	select * from dbo.Doctors
END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.spDoctors_Insert
@Name nvarchar(max),
@Photo nvarchar(max),
@RoleId int,
@Education nvarchar(max),
@DateOfBirth nvarchar(max),
@Phone nvarchar(20),
@Email nvarchar(max),
@Address nvarchar(max),
@Id int = 0 output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	insert into dbo.Doctors ([Name], Photo, RoleId, Education, DateOfBirth, Phone, Email, [Address])
	values (@Name, @Photo, @RoleId, @Education, @DateOfBirth, @Phone, @Email, @Address);

	select @Id = SCOPE_IDENTITY();
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
go

CREATE PROCEDURE dbo.spDoctors_UpdateById
@Name nvarchar(max),
@Photo nvarchar(max),
@RoleId int,
@Education nvarchar(max),
@DateOfBirth nvarchar(max),
@Phone nvarchar(20),
@Email nvarchar(max),
@Address nvarchar(max),
@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	update dbo.Doctors
	set [Name] = @Name,
	Photo = @Photo,
	RoleId = @RoleId,
	Education = @Education,
	DateOfBirth = @DateOfBirth,
	Phone = @Phone,
	Email = @Email,
	[Address] = @Address
	where Id = @Id;
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spDonors_DeleteById
@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	delete from dbo.Donors
	where Id = @Id;
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spDonors_GetAll
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	select * from dbo.Donors;
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spDonors_Insert
@Name nvarchar(max),
@Photo nvarchar(max),
@BloodGroup int,
@DateOfBirth nvarchar(max),
@Phone nvarchar(20),
@Email nvarchar(max),
@Address nvarchar(max),
@Id int = 0 output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	insert into dbo.Donors ([Name], Photo, BloodGroup, DateOfBirth, Phone, Email, [Address])
	values (@Name, @Photo, @BloodGroup, @DateOfBirth, @Phone, @Email, @Address);

	select @Id = SCOPE_IDENTITY();
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spDonors_UpdateById
@Name nvarchar(max),
@Photo nvarchar(max),
@BloodGroup int,
@DateOfBirth nvarchar(max),
@Phone nvarchar(20),
@Email nvarchar(max),
@Address nvarchar(max),
@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	update dbo.Donors
	set [Name] = @Name,
	Photo = @Photo,
	BloodGroup = @BloodGroup,
	DateOfBirth = @DateOfBirth,
	Phone = @Phone,
	Email = @Email,
	[Address] = @Address
	where Id = @Id;
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spIssue_DeleteById
@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	delete from dbo.Issue where Id = @Id;
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spIssue_UpdateById
@RecipientId int,
@BloodId int,
@BloodAmount float,
@Unit nvarchar(max),
@PricePaid money,
@DoctorInChargeId int,
@DateOfIssue nvarchar(max),
@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	update dbo.Issue
	set RecipientId = @RecipientId,
	BloodId = @BloodId,
	BloodAmount = @BloodAmount,
	Unit = @Unit,
	PricePaid = @PricePaid,
	DoctorInChargeId = @DoctorInChargeId,
	DateOfIssue = @DateOfIssue
	where Id = @Id;
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spRecipients_DeleteById
@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	delete from dbo.Recipients
	where Id = @Id;
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spRecipients_GetAll
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	select * from dbo.Recipients;
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.spRecipients_Insert
@Name nvarchar(max),
@Photo nvarchar(max),
@BloodGroup int,
@DateOfBirth nvarchar(max),
@Phone nvarchar(20),
@Email nvarchar(max),
@Address nvarchar(max),
@Id int = 0 output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	insert into dbo.Recipients ([Name], Photo, BloodGroup, DateOfBirth, Phone, Email, [Address])
	values (@Name, @Photo, @BloodGroup, @DateOfBirth, @Phone, @Email, @Address);

	select @Id = SCOPE_IDENTITY();
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spRecipients_UpdateById
@Name nvarchar(max),
@Photo nvarchar(max),
@BloodGroup int,
@DateOfBirth nvarchar(max),
@Phone nvarchar(20),
@Email nvarchar(max),
@Address nvarchar(max),
@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	update dbo.Recipients
	set [Name] = @Name,
	Photo = @Photo,
	BloodGroup = @BloodGroup,
	DateOfBirth = @DateOfBirth,
	Phone = @Phone,
	Email = @Email,
	[Address] = @Address
	where Id = @Id;
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spRoles_DeleteById
@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	delete from dbo.Roles
	where Id = @Id;
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spRoles_GetAll
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	select * from dbo.Roles;
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.spRoles_GetByDoctorId
@DoctorId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	select dbo.Roles.Id, dbo.Roles.[Name] from dbo.Roles join dbo.Doctors
	on dbo.Roles.Id = dbo.Doctors.RoleId
	where dbo.Doctors.Id = @DoctorId;
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spRoles_Insert
@Name nvarchar(max),
@Id int = 0 output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	insert into dbo.Roles ([Name])
	values (@Name);

	select @Id = SCOPE_IDENTITY();
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spRoles_UpdateById
@Name nvarchar(max),
@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	update dbo.Roles
	set [Name] = @Name
	where Id = @Id;
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spBloodCollection_DeleteById
@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	delete from dbo.BloodCollection where Id = @Id;
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spBloodCollection_GetAll
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	select * from dbo.BloodCollection;
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spBloodCollection_GetByIssueId
@IssueId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	select dbo.BloodCollection.Id, dbo.BloodCollection.DonationType, dbo.BloodCollection.DonorId, dbo.BloodCollection.BloodGroup, dbo.BloodCollection.Amount, dbo.BloodCollection.Unit, dbo.BloodCollection.DateOfCollection, dbo.BloodCollection.DoctorInChargeId
	from dbo.BloodCollection join dbo.Issue on dbo.BloodCollection.Id = dbo.Issue.BloodId
	where dbo.Issue.Id = @IssueId;

END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spBloodCollection_Insert
@DonationType int,
@DonorId int,
@BloodGroup int,
@Amount float,
@Unit nvarchar(max),
@DateOfCollection nvarchar(max),
@DoctorInChargeId int,
@Id int = 0 output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	insert into dbo.BloodCollection (DonationType, DonorId, BloodGroup, Amount, Unit, DateOfCollection, DoctorInChargeId)
	values (@DonationType, @DonorId, @BloodGroup, @Amount, @Unit, @DateOfCollection, @DoctorInChargeId);

	select @Id = SCOPE_IDENTITY();
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spBloodCollection_UpdateById
@DonationType int,
@DonorId int,
@BloodGroup int,
@Amount float,
@Unit nvarchar(max),
@DateOfCollection nvarchar(max),
@DoctorInChargeId int,
@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	update dbo.BloodCollection
	set DonationType = @DonationType,
	DonorId = @DonorId,
	BloodGroup = @BloodGroup,
	Amount = @Amount,
	Unit = @Unit,
	DateOfCollection = @DateOfCollection,
	DoctorInChargeId = @DoctorInChargeId
	where Id = @Id;
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spDoctors_GetByBloodId
@BloodId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	select dbo.Doctors.Id, dbo.Doctors.[Name], dbo.Doctors.Photo, dbo.Doctors.RoleId, dbo.Doctors.Education, dbo.Doctors.DateOfBirth , dbo.Doctors.Phone, dbo.Doctors.Email, dbo.Doctors.[Address]
	from dbo.Doctors join dbo.BloodCollection on dbo.Doctors.Id = dbo.BloodCollection.DoctorInChargeId
	where dbo.BloodCollection.Id = @BloodId;
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spDoctors_GetByIssueId
@IssueId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	select dbo.Doctors.Id, dbo.Doctors.[Name], dbo.Doctors.Photo, dbo.Doctors.RoleId, dbo.Doctors.Education, dbo.Doctors.DateOfBirth , dbo.Doctors.Phone, dbo.Doctors.Email, dbo.Doctors.[Address]
	from dbo.Doctors join dbo.Issue on dbo.Doctors.Id = dbo.Issue.DoctorInChargeId
	where dbo.Issue.Id = @IssueId;
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spDonors_GetByBloodId
@BloodId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	select dbo.Donors.Id, dbo.Donors.[Name], dbo.Donors.Photo, dbo.Donors.BloodGroup, dbo.Donors.DateOfBirth , dbo.Donors.Phone, dbo.Donors.Email, dbo.Donors.[Address]
	from dbo.Donors join dbo.BloodCollection on dbo.Donors.Id = dbo.BloodCollection.DonorId
	where dbo.BloodCollection.Id = @BloodId;
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spIssue_GetAll
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	select * from dbo.Issue;
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spIssue_Insert
@RecipientId int,
@BloodId int,
@BloodAmount float,
@Unit nvarchar(max),
@PricePaid money,
@DoctorInChargeId int,
@DateOfIssue nvarchar(max),
@Id int = 0 output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	insert into dbo.Issue (RecipientId, BloodId, BloodAmount, Unit, PricePaid, DoctorInChargeId, DateOfIssue) 
	values (@RecipientId, @BloodId, @BloodAmount, @Unit, @PricePaid, @DoctorInChargeId, @DateOfIssue); 

    select @Id = SCOPE_IDENTITY();

END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spRecipients_GetByIssueId
@IssueId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	select dbo.Recipients.Id, dbo.Recipients.[Name], dbo.Recipients.Photo, dbo.Recipients.BloodGroup, dbo.Recipients.DateOfBirth , dbo.Recipients.Phone, dbo.Recipients.Email, dbo.Recipients.[Address]
	from dbo.Recipients join dbo.Issue on dbo.Recipients.Id = dbo.Issue.RecipientId
	where dbo.Issue.Id = @IssueId;
END
GO




