using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Crawler.DAL.XmLrcDAL
{
   public class LrcExportService
    {

        //Id,XM_SongId,SongName,SingerName,AlbumName,PublishDate,SongLanguage,OriRightsHolder,SpiderTimeId,FlowStatus,Lyricist,Composer,LrcTxt,Lyric
     
       public DataTable GetLrcTable()
       {
           string sql = "select top(20) Id,XM_SongId,SongName,SingerName,AlbumName,PublishDate,SongLanguage,OriRightsHolder,Lyricist,Composer,LrcTxt,Lyric from XM_CN_EN_56876";

           DataTable dt = new DataTable();

           dt = HelperSQL.SelectData(sql,null, CommandType.Text);

           return dt;
       }

       public DataTable GetLrcTable_T1()
       {
           string sql = "select top(20) Id,XM_SongId,SongName,SingerName,AlbumName from XM_CN_EN_56876";

           DataTable dt = new DataTable();

           dt = HelperSQL.SelectData(sql, null, CommandType.Text);

           return dt;
       }





    }
}
