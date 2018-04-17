namespace AspNetCore.LdapAuthentication
{
    /// <summary>
    /// Represents options that configure LDAP authentication.
    /// </summary>
    public class LdapAuthenticationOptions
    {
        /// <summary>
        /// Gets or sets the Url on which the LDAP server is listening. 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the BindDd for the LDAP server. 
        /// </summary>
        public string BindDn { get; set; }

        /// <summary>
        /// Gets or sets the Bind Credentials for the LDAP server. 
        /// </summary>
        public string BindCredentials { get; set; }

        /// <summary>
        /// Gets or sets the Search Base with which to query the LDAP server. 
        /// </summary>
        public string SearchBase { get; set; }

        /// <summary>
        /// Gets or sets the Search Filter with which to query the LDAP server. 
        /// </summary>
        public string SearchFilter { get; set; }

        /// <summary>
        /// Gets or sets the AdminCn with which to query the LDAP server. 
        /// </summary>
        public string AdminCn { get; set; }

        /// <summary>
        /// Gets or sets the TCP port on which the LDAP server is running. 
        /// </summary>
        public int Port { get; set; } = 389;
    }
}
