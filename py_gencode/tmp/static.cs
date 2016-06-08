using System;
using System.Data;

using HiCSProvider;

namespace HiCSProvider.Demo
{
    /// <summary>
    /// implement 
    /// </summary>
    public class ProviderHelper
    {
		static ProviderImpl impl = new ProviderImpl();
				
        /// <summary>
        /// main interface 
        /// </summary>
        /// <returns></returns>
        public static ProviderImpl ProviderHelper
        {
            get
            {
                return impl;
            }
        }
#Func_List#
    }
}