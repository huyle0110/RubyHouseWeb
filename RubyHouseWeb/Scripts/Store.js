USE[treasury]
GO
/****** Object:  StoredProcedure [dbo].[insertDealerLimitMM]    Script Date: 5/31/19 2:28:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE[dbo].[insertDealerLimitMM]
@dealerId varchar(30),
@limitType varchar(20),
@dealerLimitList varchar(max),
@effectiveDate date,
@userId varchar(30),
@branchCode varchar(9),
@permissionKey	varchar(50),
@resultMessage nvarchar(4000) out,
@resultCode int out
AS
BEGIN
SET NOCOUNT, XACT_ABORT ON;

BEGIN TRY

declare @now datetime = getdate();
SET @resultCode = 99999;

--1. Check quyền
declare @isPermissionOK int;
exec checkUserPermission @userId, @permissionKey, @isPermissionOK out
IF ISNULL(@isPermissionOK , 99999) <> 0
    BEGIN
        SET @resultCode = 50016;
        RETURN;
    END

-- 2a. Ko có tham số nào được null
IF @dealerId IS NULL
OR @limitType IS NULL
OR @effectiveDate IS NULL
OR @branchCode IS NULL
OR ISNULL(ISJSON(@dealerLimitList),0)=0
    BEGIN
        SET @resultCode = 50017;
        RETURN;
    END

-- 2b.

-- Lấy thông tin dealer limit
IF ISNULL(ISJSON(@dealerLimitList),0)=1
SELECT	l.currency, l.transLimitAmount, l.dailyLimitAmount, l.maxTermByMonth
INTO #tblDealerLimitList FROM OPENJSON (@dealerLimitList)
WITH(
currency varchar(3) '$.currency',
transLimitAmount decimal(26,9) '$.transLimitAmount',
dailyLimitAmount decimal(26,9) '$.dailyLimitAmount',
maxTermByMonth decimal(6,2) '$.maxTermByMonth'
) AS l

-- Kiểm tra limit amount
IF EXISTS (SELECT 1 FROM #tblDealerLimitList as l WHERE l.transLimitAmount = 0 AND l.dailyLimitAmount = 0 AND l.currency='VND')
    BEGIN
        SET @resultCode = 50017;
        RETURN;
    END

-- 2c.
		IF @effectiveDate < cast(@now as date)
        BEGIN
            SET @resultCode = 50017;
            RETURN;
        END


-- 2d. Kiểm tra tồn tại
IF EXISTS (select 1 from [SC_DEALER_LIMIT] where [DEALER_ID] = @dealerId and [LIMIT_TYPE] = @limitType)
BEGIN
    SET @resultCode = 50002;
    RETURN;
END

IF EXISTS (select 1 from [SC_DEALER_LIMIT_INAU] where [DEALER_ID] = @dealerId and [LIMIT_TYPE] = @limitType)
BEGIN
    SET @resultCode = 50006;
    RETURN;
END

BEGIN TRANSACTION
-- 3. Thêm vào bảng [SC_DEALER_LIMIT_INAU] 
INSERT INTO [SC_DEALER_LIMIT_INAU](
            [DEALER_ID]
          ,[LIMIT_TYPE]
          ,[CURRENCY]
          ,[EFFECTIVE_DATE]
          ,[DAILY_LIMIT_AMOUNT]
          ,[TRANS_LIMIT_AMOUNT]
          ,[MAX_TERM_BY_MONTH]
          ,[MAX_TERM_BY_DAYS]
          ,[TERM_REMAIN_BY_DAYS]
          ,[SC_TERM_REMAIN_BY_YEAR]
          ,[OPEN_STATUS_LIMIT]
          ,[REVALUATION_LOSS_LIMIT]
          ,[LIMIT_NOTES]
          ,[LIMIT_STATUS]
          ,[USER_CREATED]
          ,[DATETIME_CREATED]
          ,[LAST_USER_UPDATED]
          ,[LAST_DATETIME_UPDATED]
          ,[RECORD_NO]
          ,[BRANCH_CODE]
    )
    SELECT
        @dealerId
        ,@limitType
        ,l.currency
        ,@effectiveDate
        ,l.dailyLimitAmount
        ,l.transLimitAmount
        ,l.maxTermByMonth
        ,NULL
        ,NULL
        ,NULL
        ,NULL
        ,NULL
        ,NULL
        ,'INAU'
        ,@userId
        ,@now
        ,@userId
        ,@now
        ,0
        ,@branchCode
    FROM #tblDealerLimitList as l

-- 3. Thêm vào bảng [SC_DEALER_LIMIT_INAU_LOG] 
INSERT INTO [SC_DEALER_LIMIT_INAU_LOG](
            [DEALER_ID]
          ,[LIMIT_TYPE]
          ,[CURRENCY]
          ,[EFFECTIVE_DATE]
          ,[DAILY_LIMIT_AMOUNT]
          ,[TRANS_LIMIT_AMOUNT]
          ,[MAX_TERM_BY_MONTH]
          ,[MAX_TERM_BY_DAYS]
          ,[TERM_REMAIN_BY_DAYS]
          ,[SC_TERM_REMAIN_BY_YEAR]
          ,[OPEN_STATUS_LIMIT]
          ,[REVALUATION_LOSS_LIMIT]
          ,[LIMIT_NOTES]
          ,[LIMIT_STATUS]
          ,[USER_CREATED]
          ,[DATETIME_CREATED]
          ,[LAST_USER_UPDATED]
          ,[LAST_DATETIME_UPDATED]
          ,[RECORD_NO]
          ,[BRANCH_CODE]
    )
    SELECT
        @dealerId
        ,@limitType
        ,l.currency
        ,@effectiveDate
        ,l.dailyLimitAmount
        ,l.transLimitAmount
        ,l.maxTermByMonth
        ,NULL
        ,NULL
        ,NULL
        ,NULL
        ,NULL
        ,NULL
        ,'INAU'
        ,@userId
        ,@now
        ,@userId
        ,@now
        ,0
        ,@branchCode
    FROM #tblDealerLimitList as l

COMMIT TRAN;
SET @resultCode = 0;
END TRY

BEGIN CATCH

IF @@TRANCOUNT > 0 ROLLBACK TRAN;
SET @resultCode = ERROR_NUMBER();

END CATCH			
            	
END


USE [treasury]
GO
/****** Object:  StoredProcedure [dbo].[searchProductMMHis]    Script Date: 5/31/19 2:29:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Create the stored procedure in the specified schema
ALTER PROCEDURE [dbo].[searchProductMMHis]
@pageNumber int,
@pageSize int,
@mmProductId varchar(22),
@mmProductName varchar(32),
@mmtradeType varchar(20),
@mmProductType varchar(20),
@dealerId varchar(30),
@userUpdated varchar(50),
@fromDate datetime,
@toDate datetime,
@status varchar(max),

@resultMessage nvarchar(max) out,
@resultCode int out
AS
BEGIN
SET NOCOUNT ON;
SET @resultCode = 99999;

declare @SQLCount nvarchar(max);
declare @SQLQuery nvarchar(max);
declare @SQLFrom nvarchar(2000);
declare @SQLWhere nvarchar(2000);
declare @SQLPage nvarchar(2000);
declare @SQLJSon nvarchar(2000);

Declare @ParamDefinitionCount AS nVarchar(2000);
Declare @ParamDefinition AS nVarchar(2000);
declare @OFFSET int;
declare @rowTotal int;
--declare @pageTotal int;

	IF @pageSize < 1
    SET @pageSize = 20;

	IF @pageNumber < 1
    SET @pageNumber = 1;

IF ISNULL(ISJSON(@status),0)=1
SELECT	l.statusId
    INTO #tblStatusList FROM OPENJSON (@status) 
    WITH(
        statusId varchar(10) '$.statusId'
    ) AS l

SET @OFFSET  = (@pageNumber - 1) * @pageSize;

	IF @OFFSET <= 1 
		SET @OFFSET = 0;

	SET @SQLCount = 'SELECT @rowTotal = count(*)'

	SET @SQLQuery = N'SET @resultMessage = (select
							(Select @rowTotal) as totalRecord,
							(SELECT 
                            MDMM_LOAN_DEPOSIT_HIS.MM_PRODUCT_ID as mmProductId,
                            MDMM_LOAN_DEPOSIT_HIS.MM_PRODUCT_NAME as mmProductName,
                            MDMM_LOAN_DEPOSIT_HIS.MM_TRADE_TYPE as mmtradeType,
                            MDMM_LOAN_DEPOSIT_HIS.MM_PRODUCT_TYPE as mmProductType,
							MDMM_LOAN_DEPOSIT_HIS.[MM_PRODUCT_STATUS] as mmProductStatus,
							CASE MDMM_LOAN_DEPOSIT_HIS.[MM_PRODUCT_STATUS]
								 WHEN ''A'' THEN N''Hiệu lực''
								 WHEN ''RV'' THEN N''Hủy''
							END AS mmProductStatusName,
                            LAST_USER_UPDATED as lastUserUpdated,
                            LAST_DATETIME_UPDATED as lastDatetimeUpdated';

	SET @SQLFrom = ' FROM MDMM_LOAN_DEPOSIT_HIS';	
					
	SET @SQLWhere = ' WHERE (1=1) ';

	SET @SQLPage = ' ORDER BY LAST_DATETIME_UPDATED DESC OFFSET @OFFSET ROWS FETCH NEXT @pageSize ROWS ONLY';

	SET @SQLJSon = ' FOR JSON PATH,INCLUDE_NULL_VALUES) as resultList FOR JSON PATH,INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)';

	IF ISNULL(ISJSON(@status),0)=1
		BEGIN
			SET @SQLWhere = concat(@SQLWhere,' AND  [MM_PRODUCT_STATUS] in (select statusId from #tblStatusList)');
		END
	IF @mmProductId IS NOT NULL
		BEGIN
			SET @mmProductId = '%'+@mmProductId+'%'
			SET @SQLWhere = @SQLWhere + ' AND MM_PRODUCT_ID like @mmProductId ';
		END

	IF @mmProductName IS NOT NULL
		BEGIN
            SET @mmProductName = '%'+@mmProductName+'%'
			SET @SQLWhere = @SQLWhere + ' AND MM_PRODUCT_NAME like @mmProductName ';
		END

	IF @mmProductType IS NOT NULL
		BEGIN
			SET @SQLWhere = @SQLWhere + ' AND [MM_PRODUCT_TYPE] = @mmProductType ';
		END

	IF @mmtradeType IS NOT NULL
		BEGIN
			SET @SQLWhere = @SQLWhere + ' AND [MM_TRADE_TYPE] = @mmtradeType ';
		END
	IF @dealerID IS NOT NULL
		BEGIN          
			SET @SQLWhere = @SQLWhere + ' and EXISTS(SELECT 1 FROM MDMM_LOAN_DEPOSIT_X_DEALER WHERE MDMM_LOAN_DEPOSIT_X_DEALER.MM_PRODUCT_ID = MDMM_LOAN_DEPOSIT_HIS.MM_PRODUCT_ID AND MDMM_LOAN_DEPOSIT_X_DEALER.DEALER_ID =@dealerID) ';
		END

	IF @userUpdated IS NOT NULL
		BEGIN
			SET @SQLWhere = @SQLWhere + ' AND [LAST_USER_UPDATED] = @userUpdated';
		END
	IF @fromDate IS NOT NULL
		BEGIN
			SET @SQLWhere = @SQLWhere + ' AND convert(date, [LAST_DATETIME_UPDATED]) >= @fromDate';
		END

	IF @toDate IS NOT NULL
		BEGIN
			SET @SQLWhere = @SQLWhere + ' AND convert(date, [LAST_DATETIME_UPDATED]) <= @toDate';
END

 

SET @SQLCount = @SQLCount + @SQLFrom + @SQLWhere;

print @SQLCount


--print @SQLCount;

SET @ParamDefinitionCount = '@mmProductId varchar(22),
                @mmProductName varchar(32),
                @mmtradeType varchar(20),
                @mmProductType varchar(20),
                @dealerId varchar(30),
                @userUpdated varchar(50),
                @fromDate datetime,
                @toDate datetime,
                @status varchar(max),
                @rowTotal int out';
	
BEGIN TRY
	
Execute sp_Executesql @SQLCount, 
    @ParamDefinitionCount, 
    @mmProductId,
    @mmProductName,
    @mmtradeType,
    @mmProductType,
    @dealerId,
    @userUpdated,
    @fromDate,
    @toDate,
    @status,			
    @rowTotal out
	

IF @rowTotal > 0
BEGIN
	
SET @SQLQuery = @SQLQuery + @SQLFrom + @SQLWhere + @SQLPage + @SQLJSon;

SET @ParamDefinition = '@pageSize	int,
                    @rowTotal int,
                    @mmProductId varchar(22),
                    @mmProductName varchar(32),
                    @mmtradeType varchar(20),
                    @mmProductType varchar(20),
                    @dealerId varchar(30),
                    @userUpdated varchar(50),
                    @fromDate datetime,
                    @toDate datetime,
                    @status varchar(max),
                    @OFFSET int,
                    @resultMessage nvarchar(max) out';

Execute sp_Executesql @SQLQuery, 
@ParamDefinition, 
@pageSize,
@rowTotal,
@mmProductId,
@mmProductName,
@mmtradeType,
@mmProductType,
@dealerId,
@userUpdated,
@fromDate,
@toDate,
@status,
@OFFSET,
@resultMessage out

SET @resultCode = 0;
    	
END

ELSE
BEGIN
SET @resultCode = 50004;
END


END TRY
BEGIN CATCH
;throw
SET @resultCode = ERROR_NUMBER();
RETURN;
END CATCH
END



USE [treasury]
GO
/****** Object:  StoredProcedure [dbo].[editDealerLimitMM]    Script Date: 5/31/19 2:30:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[editDealerLimitMM]
@dealerId varchar(30),
@limitType varchar(20),
@dealerLimitList varchar(max),
@effectiveDate date,
@isInau tinyint,
@userId varchar(30),
@branchCode varchar(9),
@permissionKey	varchar(50),
@resultMessage nvarchar(4000) out,
@resultCode int out
AS
BEGIN
SET NOCOUNT, XACT_ABORT ON;

BEGIN TRY

declare @now datetime = getdate();
SET @resultCode = 99999;

-- 1. Check quyền
declare @isPermissionOK int;
exec checkUserPermission @userId, @permissionKey, @isPermissionOK out
			IF ISNULL(@isPermissionOK , 99999) <> 0
        BEGIN
            SET @resultCode = 50016;
            RETURN;
        END

-- 2a. Ko có tham số nào được null
IF @dealerId IS NULL
    OR @limitType IS NULL
    OR ISNULL(ISJSON(@dealerLimitList),0)=0
    OR @effectiveDate IS NULL
    OR @branchCode IS NULL
        BEGIN
            SET @resultCode = 50017;
            RETURN;
        END

-- 2b.

-- Lấy thông tin dealer limit
IF ISNULL(ISJSON(@dealerLimitList),0)=1
SELECT	l.currency, l.transLimitAmount, l.dailyLimitAmount, l.maxTermByMonth
INTO #tblDealerLimitList FROM OPENJSON (@dealerLimitList)
WITH(
    currency varchar(3) '$.currency',
    transLimitAmount decimal(20,4) '$.transLimitAmount',
    dailyLimitAmount decimal(20,4) '$.dailyLimitAmount',
    maxTermByMonth decimal(6,2) '$.maxTermByMonth'
) AS l

-- Kiểm tra limit amount
IF EXISTS (SELECT 1 FROM #tblDealerLimitList as l WHERE l.transLimitAmount = 0 AND l.dailyLimitAmount = 0 AND l.currency='VND')
        BEGIN
            SET @resultCode = 50017;
            RETURN;
        END

-- 2c.
		IF @effectiveDate < cast(@now as date)
				BEGIN
					SET @resultCode = 50017;
					RETURN;
				END

		-- 3. Kiểm tra tồn tại

		IF  @isInau = 0 AND EXISTS (select 1 from [SC_DEALER_LIMIT_INAU] where [DEALER_ID] = @dealerId 
													AND [LIMIT_TYPE] = @limitType)
		BEGIN
			SET @resultCode = 50006;
			RETURN;
		END

		-- 4. 

		IF @isInau = 0 -- Edit bang da duyet
		BEGIN
		select * into #DEALER_LIMIT from [SC_DEALER_LIMIT] where [DEALER_ID] = @dealerId
							and [LIMIT_TYPE] = @limitType;

			update d
			SET
				 [TRANS_LIMIT_AMOUNT] = l.transLimitAmount
				,[DAILY_LIMIT_AMOUNT] = l.dailyLimitAmount
				,[MAX_TERM_BY_MONTH] = l.maxTermByMonth
				,[EFFECTIVE_DATE] = @effectiveDate
				,[LIMIT_STATUS] = 'INAU'
				,[LAST_USER_UPDATED] = @userId
				,[LAST_DATETIME_UPDATED] = @now
				,[BRANCH_CODE] = @branchCode
			FROM #DEALER_LIMIT as d
			INNER JOIN #tblDealerLimitList as l 
			ON d.CURRENCY = l.currency

		END

		BEGIN TRANSACTION

		IF @isInau = 0 -- Edit bang da duyet
		BEGIN

		-- Thêm vào bảng [SC_DEALER_LIMIT_INAU] 
		INSERT INTO [SC_DEALER_LIMIT_INAU](
		[DEALER_ID] ,[LIMIT_TYPE] ,[CURRENCY] ,[EFFECTIVE_DATE] ,[DAILY_LIMIT_AMOUNT] 
		,[TRANS_LIMIT_AMOUNT] ,[MAX_TERM_BY_MONTH] ,[MAX_TERM_BY_DAYS] ,[TERM_REMAIN_BY_DAYS] 
		,[SC_TERM_REMAIN_BY_YEAR] ,[OPEN_STATUS_LIMIT] ,[REVALUATION_LOSS_LIMIT] ,[LIMIT_NOTES] 
		,[LIMIT_STATUS] ,[LAST_USER_UPDATED] ,[LAST_DATETIME_UPDATED] ,[RECORD_NO] ,[BRANCH_CODE]
		,[USER_APPROVED] ,[DATETIME_APPROVED])
		SELECT 
		[DEALER_ID] ,[LIMIT_TYPE] ,[CURRENCY] ,[EFFECTIVE_DATE] ,[DAILY_LIMIT_AMOUNT] 
		,[TRANS_LIMIT_AMOUNT] ,[MAX_TERM_BY_MONTH] ,[MAX_TERM_BY_DAYS] ,[TERM_REMAIN_BY_DAYS] 
		,[SC_TERM_REMAIN_BY_YEAR] ,[OPEN_STATUS_LIMIT] ,[REVALUATION_LOSS_LIMIT] ,[LIMIT_NOTES] 
		,[LIMIT_STATUS] ,[LAST_USER_UPDATED] ,[LAST_DATETIME_UPDATED] ,[RECORD_NO] ,[BRANCH_CODE]
		,[USER_APPROVED] ,[DATETIME_APPROVED]
		FROM #DEALER_LIMIT

		--Thêm vào bảng [SC_DEALER_LIMIT_INAU_LOG] 
		INSERT INTO [SC_DEALER_LIMIT_INAU_LOG](
		[DEALER_ID] ,[LIMIT_TYPE] ,[CURRENCY] ,[EFFECTIVE_DATE] ,[DAILY_LIMIT_AMOUNT] 
		,[TRANS_LIMIT_AMOUNT] ,[MAX_TERM_BY_MONTH] ,[MAX_TERM_BY_DAYS] ,[TERM_REMAIN_BY_DAYS] 
		,[SC_TERM_REMAIN_BY_YEAR] ,[OPEN_STATUS_LIMIT] ,[REVALUATION_LOSS_LIMIT] ,[LIMIT_NOTES] 
		,[LIMIT_STATUS] ,[LAST_USER_UPDATED] ,[LAST_DATETIME_UPDATED] ,[RECORD_NO] ,[BRANCH_CODE]
		,[USER_APPROVED] ,[DATETIME_APPROVED])
		SELECT 
		[DEALER_ID] ,[LIMIT_TYPE] ,[CURRENCY] ,[EFFECTIVE_DATE] ,[DAILY_LIMIT_AMOUNT] 
		,[TRANS_LIMIT_AMOUNT] ,[MAX_TERM_BY_MONTH] ,[MAX_TERM_BY_DAYS] ,[TERM_REMAIN_BY_DAYS] 
		,[SC_TERM_REMAIN_BY_YEAR] ,[OPEN_STATUS_LIMIT] ,[REVALUATION_LOSS_LIMIT] ,[LIMIT_NOTES] 
		,[LIMIT_STATUS] ,[LAST_USER_UPDATED] ,[LAST_DATETIME_UPDATED] ,[RECORD_NO] ,[BRANCH_CODE]
		,[USER_APPROVED] ,[DATETIME_APPROVED]
		FROM #DEALER_LIMIT

		END
		ELSE -- Edit bang chua duyet
		BEGIN
			
			-- update [SC_DEALER_LIMIT_INAU] 
			UPDATE d
			set 
				[TRANS_LIMIT_AMOUNT] = l.transLimitAmount
				,[DAILY_LIMIT_AMOUNT] = l.dailyLimitAmount
				,[MAX_TERM_BY_MONTH] = l.maxTermByMonth
				,[EFFECTIVE_DATE] = @effectiveDate
				,[LIMIT_STATUS] = 'INAU'
				,[LAST_USER_UPDATED] = @userId
				,[LAST_DATETIME_UPDATED] = @now
				,[BRANCH_CODE] = @branchCode
			FROM [SC_DEALER_LIMIT_INAU]  as d
			INNER JOIN #tblDealerLimitList as l 
			ON d.CURRENCY = l.currency
			WHERE d.[DEALER_ID] = @dealerId and d.[LIMIT_TYPE] = @limitType

			--Thêm vào bảng [SC_DEALER_LIMIT_INAU_LOG] 
			INSERT INTO [SC_DEALER_LIMIT_INAU_LOG](
			[DEALER_ID] ,[LIMIT_TYPE] ,[CURRENCY] ,[EFFECTIVE_DATE] ,[DAILY_LIMIT_AMOUNT] 
			,[TRANS_LIMIT_AMOUNT] ,[MAX_TERM_BY_MONTH] ,[MAX_TERM_BY_DAYS] ,[TERM_REMAIN_BY_DAYS] 
			,[SC_TERM_REMAIN_BY_YEAR] ,[OPEN_STATUS_LIMIT] ,[REVALUATION_LOSS_LIMIT] ,[LIMIT_NOTES] 
			,[LIMIT_STATUS] ,[LAST_USER_UPDATED] ,[LAST_DATETIME_UPDATED] ,[RECORD_NO] ,[BRANCH_CODE]
			,[USER_APPROVED] ,[DATETIME_APPROVED])
			SELECT 
			[DEALER_ID] ,[LIMIT_TYPE] ,[CURRENCY] ,[EFFECTIVE_DATE] ,[DAILY_LIMIT_AMOUNT] 
			,[TRANS_LIMIT_AMOUNT] ,[MAX_TERM_BY_MONTH] ,[MAX_TERM_BY_DAYS] ,[TERM_REMAIN_BY_DAYS] 
			,[SC_TERM_REMAIN_BY_YEAR] ,[OPEN_STATUS_LIMIT] ,[REVALUATION_LOSS_LIMIT] ,[LIMIT_NOTES] 
			,[LIMIT_STATUS] ,[LAST_USER_UPDATED] ,[LAST_DATETIME_UPDATED] ,[RECORD_NO] ,[BRANCH_CODE]
			,[USER_APPROVED] ,[DATETIME_APPROVED]
			FROM [SC_DEALER_LIMIT_INAU]
			WHERE [DEALER_ID] = @dealerId and [LIMIT_TYPE] = @limitType;
		END

		COMMIT TRAN;
		SET @resultCode = 0;
	END TRY

	BEGIN CATCH

		IF @@TRANCOUNT > 0 ROLLBACK TRAN;
		SET @resultCode = ERROR_NUMBER();

	END CATCH			
						
END





USE [treasury]
GO
/****** Object:  StoredProcedure [dbo].[deleteYieldMargin]    Script Date: 5/31/19 2:31:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[deleteYieldMargin]
@bondTypeID	varchar(20),
@currencyID varchar(3),
@yieldID	varchar(10),
@isInau tinyint,
@userId	varchar(50),
@branchCode varchar(9),
@permissionKey	varchar(50),
@resultMessage nvarchar(4000) out,
@resultCode int out
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY

	SET @resultCode = 99999;

	declare @isPermissionOK int;

	exec checkUserPermission @userId, @permissionKey, @isPermissionOK out

		IF isnull(@isPermissionOK , 99999) <> 0
			BEGIN
				SET @resultCode = 50016;
				RETURN;
			END

	IF @bondTypeID is null OR @yieldID is null OR @currencyID is null OR @isInau is null 
	OR @branchCode is null
	BEGIN
			SET @resultCode = 50017;
			RETURN;
	END

	--IF NOT EXISTS (select 1 from [SC_TRANS_TYPE_LIMIT] where [SC_TRANS_TYPE_ID] = @yieldID) 
	--	BEGIN
	--		SET @resultCode = 50004;
	--		RETURN;
	--	END

		declare @now datetime = getdate();

		BEGIN TRANSACTION

		IF @isInau = 0 -- Trường hợp 1: cập nhật bảng chính
		BEGIN

			-- Insert bảng HIS 
			INSERT INTO [SC_YIELD_INTEREST_RATE_MARGIN_HIS]
			(
				[YIELD_ID]
				,[BOND_TYPE_CODE]
				,[CURRENCY]
				,[MIN_BID_LIMIT]
				,[MAX_ASK_LIMIT]
				,[MIN_OPERATOR]
				,[MAX_OPERATOR]
				,[LAST_USER_UPDATED]
				,[LAST_DATETIME_UPDATED]
				,[RECORD_NO]
				,[USER_APPROVED]
				,[DATETIME_APPROVED]
				,[YIELD_MARGIN_STATUS]
				,[BRANCH_CODE]
			)
			SELECT
			[YIELD_ID]
				,[BOND_TYPE_CODE]
				,[CURRENCY]
				,[MIN_BID_LIMIT]
				,[MAX_ASK_LIMIT]
				,[MIN_OPERATOR]
				,[MAX_OPERATOR]
				,@userId
				,@now
				,[RECORD_NO]
				,[USER_APPROVED]
				,[DATETIME_APPROVED]
				,'RV'
				,@branchCode
			FROM [SC_YIELD_INTEREST_RATE_MARGIN]
			WHERE [YIELD_ID] = @yieldID and [BOND_TYPE_CODE] = @bondTypeID
				  and [CURRENCY] =  @currencyID;
			
			-- Xóa dữ liệu trong bảng chính
			DELETE FROM [SC_YIELD_INTEREST_RATE_MARGIN]
			WHERE [YIELD_ID] = @yieldID and [BOND_TYPE_CODE] = @bondTypeID
				  and [CURRENCY] =  @currencyID

		END
		ELSE -- Trường hợp cập nhật bảng chờ duyệt
		BEGIN
				-- Insert bảng LOG
				INSERT INTO [SC_YIELD_INTEREST_RATE_MARGIN_INAU_LOG]
				(  [YIELD_ID]
				,[BOND_TYPE_CODE]
				,[CURRENCY]
				,[MIN_BID_LIMIT]
				,[MAX_ASK_LIMIT]
				,[MIN_OPERATOR]
				,[MAX_OPERATOR]
				,[LAST_USER_UPDATED]
				,[LAST_DATETIME_UPDATED]
				,[RECORD_NO]
				,[YIELD_MARGIN_STATUS]
				,[USER_APPROVED]
				,[DATETIME_APPROVED]
				,[BRANCH_CODE]
				)
				SELECT
					 [YIELD_ID]
					 ,[BOND_TYPE_CODE]
					 ,[CURRENCY]      
					 ,[MIN_BID_LIMIT]      
					 ,[MAX_ASK_LIMIT]      
					 ,[MIN_OPERATOR]      
					 ,[MAX_OPERATOR]      
					 ,@userId      
					 ,@now    
					 ,[RECORD_NO]      
					 ,'DEL'    
					 ,[USER_APPROVED]      
					 ,[DATETIME_APPROVED]
					 ,@branchCode
				FROM [SC_YIELD_INTEREST_RATE_MARGIN_INAU]
				WHERE [YIELD_ID] = @yieldID and [BOND_TYPE_CODE] = @bondTypeID
				  and [CURRENCY] =  @currencyID;

				-- Xóa dữ liệu trong bảng INAU
				delete from [SC_YIELD_INTEREST_RATE_MARGIN_INAU]
				WHERE [YIELD_ID] = @yieldID and [BOND_TYPE_CODE] = @bondTypeID
				  and [CURRENCY] =  @currencyID;
	
		END

		COMMIT TRAN;
		SET @resultCode = 0;

	END TRY

	BEGIN CATCH
		if @@TRANCOUNT > 0 rollback tran;
		set @resultCode = ERROR_NUMBER();
	END CATCH			
						
END
