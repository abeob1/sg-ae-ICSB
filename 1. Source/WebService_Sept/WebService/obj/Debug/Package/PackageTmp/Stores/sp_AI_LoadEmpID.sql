--sp_AI_LoadEmpID 'duccv'
create proc sp_AI_LoadEmpID
@EmpID nvarchar(100)
as
-----------EMPLOYEE-------------
select T0.empID,isnull(T0.manager,T0.empID) managerID,T0.U_UserName LoginID, 
isnull(T0.firstName,'') +' ' + isnull(T0.middleName,'') +' '+ isnull(T0.lastName,'') EmployeeName,
T0.U_DefaultWhs,T3.WhsName DefaultWhsName, T2.Name Hotel,T0.dept, T4.Name DepartmentName,
T0.email EmployeeEmail,
------------MANAGER------------
isnull(T1.firstName,'') +' '+ isnull(T1.middleName,'') +' '+ isnull(T1.lastName,'') ManagerName,
T1.U_UserName ManagerLoginID,T1.email ManagerEmail,

-----------COMPANY---------------
(select CompnyName from OADM) CompanyName
from ohem T0
left join OHEM T1 on T0.manager = T1.empID 
left join [@HOTEL] T2 on T0.U_Hotel COLLATE DATABASE_DEFAULT=T2.Code COLLATE DATABASE_DEFAULT
left join OWHS T3 with(nolock) on T3.WhsCode COLLATE DATABASE_DEFAULT =T0.U_DefaultWhs COLLATE DATABASE_DEFAULT
left join OUDP T4 on T4.Code=T0.dept
where T0.U_UserName=@EmpID


