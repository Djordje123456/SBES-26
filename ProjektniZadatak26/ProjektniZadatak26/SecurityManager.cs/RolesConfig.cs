namespace SecurityManager.cs
{
    //vraca permisije sa odredjenu rolu
    public class RolesConfig
    {
        static string path = @"~\..\..\..\..\SecurityManager\RolesConfigFile.resx";
        public static bool GetPermissions(string rolename, out string[] permissions)
        {
            permissions = new string[10];
            string permissionString = string.Empty;

            permissionString = (string)RolesConfigFile.ResourceManager.GetObject(rolename);
            if (permissionString != null)
            {
                permissions = permissionString.Split(',');
                return true;
            }
            return false;

        }
    }
}
