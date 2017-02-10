alter proc sp_clearUser
as
delete from aspnet_UsersInRoles
delete from Users_Default
delete from aspnet_Membership
delete from aspnet_Profile
delete from aspnet_Users

delete from XML_Off
delete from XML_Log

