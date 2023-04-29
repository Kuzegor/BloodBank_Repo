
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.spBloodCollection_UpdateAmountById
@Amount float,
@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	update dbo.BloodCollection
	set Amount = @Amount
	where Id = @Id;
END
GO
