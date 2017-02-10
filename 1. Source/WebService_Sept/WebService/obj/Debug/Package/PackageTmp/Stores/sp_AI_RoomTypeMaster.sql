alter proc sp_AI_RoomTypeMaster
as
select tm.ItemCode, tm.ItemName, t1.Price as BasePrice, '' as Items, tm.U_Hotel
from OITM tm join ITM1 t1 on t1.ItemCode = tm.ItemCode and t1.PriceList = 1 and tm.U_POSFlag = 'Y' and tm.U_ItemSource = 'ROOM'