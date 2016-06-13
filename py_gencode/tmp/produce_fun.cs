    
        /// <summary>
        /// #script# 
        /// </summary>#args#
        /// <returns></returns>
        public #returns# #name#(#paramers#)
        {
			IDictionary<string, string> dic = new Dictionary<string, string>();
#AddDic#
            return #operator#("#SQL_ID#", dic);
        }
	