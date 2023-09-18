namespace Data.Migrations
{
    using Domain.Entities;
    using Domain.Enum;
    using Domain.Util;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Context.ModelContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Context.ModelContext context)
        {
            #region PermissionEnum
            context.Permission.AddOrUpdate(
                data => data.Id,
                new Permission() { Id = 1, ClaimType = PropertyDescription.GetEnumDescription(TypePermissionEnum.Administrators), ClaimValue = PropertyDescription.GetEnumDescription(ValuePermissionEnum.Create), IdUser = null },
                new Permission() { Id = 2, ClaimType = PropertyDescription.GetEnumDescription(TypePermissionEnum.Administrators), ClaimValue = PropertyDescription.GetEnumDescription(ValuePermissionEnum.Consult), IdUser = null },
                new Permission() { Id = 3, ClaimType = PropertyDescription.GetEnumDescription(TypePermissionEnum.Administrators), ClaimValue = PropertyDescription.GetEnumDescription(ValuePermissionEnum.Update), IdUser = null },
                new Permission() { Id = 4, ClaimType = PropertyDescription.GetEnumDescription(TypePermissionEnum.Administrators), ClaimValue = PropertyDescription.GetEnumDescription(ValuePermissionEnum.Deactivate), IdUser = null },
                new Permission() { Id = 5, ClaimType = PropertyDescription.GetEnumDescription(TypePermissionEnum.Secretaries), ClaimValue = PropertyDescription.GetEnumDescription(ValuePermissionEnum.Create), IdUser = null },
                new Permission() { Id = 6, ClaimType = PropertyDescription.GetEnumDescription(TypePermissionEnum.Secretaries), ClaimValue = PropertyDescription.GetEnumDescription(ValuePermissionEnum.Consult), IdUser = null },
                new Permission() { Id = 7, ClaimType = PropertyDescription.GetEnumDescription(TypePermissionEnum.Secretaries), ClaimValue = PropertyDescription.GetEnumDescription(ValuePermissionEnum.Update), IdUser = null },
                new Permission() { Id = 8, ClaimType = PropertyDescription.GetEnumDescription(TypePermissionEnum.Secretaries), ClaimValue = PropertyDescription.GetEnumDescription(ValuePermissionEnum.Deactivate), IdUser = null },
                new Permission() { Id = 9, ClaimType = PropertyDescription.GetEnumDescription(TypePermissionEnum.Readers), ClaimValue = PropertyDescription.GetEnumDescription(ValuePermissionEnum.Create), IdUser = null },
                new Permission() { Id = 10, ClaimType = PropertyDescription.GetEnumDescription(TypePermissionEnum.Readers), ClaimValue = PropertyDescription.GetEnumDescription(ValuePermissionEnum.Consult), IdUser = null },
                new Permission() { Id = 11, ClaimType = PropertyDescription.GetEnumDescription(TypePermissionEnum.Readers), ClaimValue = PropertyDescription.GetEnumDescription(ValuePermissionEnum.Update), IdUser = null },
                new Permission() { Id = 12, ClaimType = PropertyDescription.GetEnumDescription(TypePermissionEnum.Readers), ClaimValue = PropertyDescription.GetEnumDescription(ValuePermissionEnum.Deactivate), IdUser = null },
                new Permission() { Id = 13, ClaimType = PropertyDescription.GetEnumDescription(TypePermissionEnum.Talents), ClaimValue = PropertyDescription.GetEnumDescription(ValuePermissionEnum.Create), IdUser = null },
                new Permission() { Id = 14, ClaimType = PropertyDescription.GetEnumDescription(TypePermissionEnum.Talents), ClaimValue = PropertyDescription.GetEnumDescription(ValuePermissionEnum.Consult), IdUser = null },
                new Permission() { Id = 15, ClaimType = PropertyDescription.GetEnumDescription(TypePermissionEnum.Talents), ClaimValue = PropertyDescription.GetEnumDescription(ValuePermissionEnum.Update), IdUser = null },
                new Permission() { Id = 16, ClaimType = PropertyDescription.GetEnumDescription(TypePermissionEnum.Talents), ClaimValue = PropertyDescription.GetEnumDescription(ValuePermissionEnum.Deactivate), IdUser = null }
                );
            #endregion
            #region Profile
            context.Profile.AddOrUpdate(
                data => data.Id,
                new Profile()
                {
                    Id = (int)ProfileEnum.Administrator,
                    Name = PropertyDescription.GetEnumDescription(ProfileEnum.Administrator),
                    Active = ((int)GenericStatusEnum.Active).ToString()
                },
                new Profile()
                {
                    Id = (int)ProfileEnum.Secretary,
                    Name = PropertyDescription.GetEnumDescription(ProfileEnum.Secretary),
                    Active = ((int)GenericStatusEnum.Active).ToString()
                },
                new Profile()
                {
                    Id = (int)ProfileEnum.Reader,
                    Name = PropertyDescription.GetEnumDescription(ProfileEnum.Reader),
                    Active = ((int)GenericStatusEnum.Active).ToString()
                }
            );
            #endregion

            #region ProfilePermission
            context.ProfilePermission.AddOrUpdate(
            #region Administratator
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Administrator, IdPermission = 1 },
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Administrator, IdPermission = 2 },
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Administrator, IdPermission = 5 },
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Administrator, IdPermission = 6 },
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Administrator, IdPermission = 7 },
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Administrator, IdPermission = 8 },
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Administrator, IdPermission = 9 },
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Administrator, IdPermission = 10 },
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Administrator, IdPermission = 11 },
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Administrator, IdPermission = 12 },
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Administrator, IdPermission = 13 },
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Administrator, IdPermission = 14 },
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Administrator, IdPermission = 15 },
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Administrator, IdPermission = 16 },
            #endregion
            #region Secretary
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Secretary, IdPermission = 5 },
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Secretary, IdPermission = 6 },
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Secretary, IdPermission = 7 },
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Secretary, IdPermission = 9 },
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Secretary, IdPermission = 10 },
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Secretary, IdPermission = 11 },
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Secretary, IdPermission = 13 },
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Secretary, IdPermission = 14 },
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Secretary, IdPermission = 15 },
            #endregion
            #region Reader
                new ProfilePermission() { IdProfile = (int)ProfileEnum.Reader, IdPermission = 14 }
                #endregion
            );
            #endregion
        }
    }
}