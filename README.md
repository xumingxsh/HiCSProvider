# HiCSProvider
一个用VS2013开发的C#程序,目标是简化数据库访问操作.具体实现:将SQL语句存储到某个文件夹下的XML文件中,外部以SQL语句的唯一标识查找到相关语句并执行.本程序同时还提供通过将XML中的SQL标识映射为一个Rest接口实现Rest访问.
本程序中,我使用到了我的程序:HiSQLManager(https://github.com/xumingxsh/HiSQLManager),用于缓存XML中的SQL信息,HiCSDB(https://github.com/xumingxsh/HiCSDB):用于与数据库交互
(注: 在本程序中,我没有使用HiCSDB扩展数据库支持的功能,如果需要可以添加)
