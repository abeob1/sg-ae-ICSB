create function [dbo].[usf_getChildList](
@ItemCode nvarchar(20)
) returns nvarchar(500)
as
begin
	declare @str nvarchar(500)
	set @str = ''
	select @str = @str + oi.ItemName + ', ' from ITT1 it1 join OITM oi on it1.Code = oi.ItemCode 
	where it1.Father = @ItemCode
	if LEN(@str)>1
	return left (@str, len(@str)-1)
	
	return ''
end

-- select dbo.usf_getChildList('STD-KING')

--select * from OITM

