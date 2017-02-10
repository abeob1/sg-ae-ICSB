alter proc sp_AI_AddonMaster
as
select tm.ItemCode, tm.ItemName, t1.Price as BasePrice, '' as Items ,tm.U_IsTax IsTaxIncluded,
10 SCPercent, 6 VATPercent,
case when isnull(tm.U_IsTax,'N')='N' then t1.Price*1.16 
		else t1.Price
	end PriceAfterTaxes,tm.U_RevenueCode ItemGroup,
tm.U_Hotel
from OITM tm	
join ITM1 t1 on t1.ItemCode = tm.ItemCode
where t1.PriceList = 1 and tm.U_POSFlag = 'Y' and tm.U_ItemSource = 'SVC'
	
