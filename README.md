# WorkJob
1. 使用VS发布网站，在IIS中新增站点，将新站点指向VS发布后的文件夹。
2. 本站点使用EXCEL文件作为只读数据源，如需更新数据，需要将Excels/Data.xlsx文件进行更新后重启服务，或者调用Index/ReloadData，该API为Post。
