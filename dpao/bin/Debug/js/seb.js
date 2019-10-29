function ODDS(s) {
    try {
       
       var arr = eval(s);
        // eval(s);
        // return arr[3][0].toString();
        var Rows='';
        //遍历联赛
        for (var i = 0; i < arr[3][0].length; i++) {
             var lianSaiId = arr[3][0][i][0]; //联赛id
             var lianSaiName = arr[3][0][i][1];//联赛名
           
               
                
            
             //遍历球队
            for (var j = 0; j < arr[3][1].length; j++) {
                if (lianSaiId == arr[3][1][j][2])//球队id
                { 
                  
                    var biSaiId = arr[3][1][j][0]; //比赛场次id
                    var biSaiIdNew='';
                    var zhuDuiName = arr[3][1][j][3];//主队
                    var keDuiName = arr[3][1][j][4];//客队
                   
                    
                    //遍历盘口
                    for (var k = 0; k < arr[3][2].length; k++) {
                        if (biSaiId == arr[3][2][k][1]) {
                            biSaiIdNew = arr[3][2][k][0];
                            break;
                        }
                    }
                    
                    var showString='';
                    showString =  lianSaiName
                                + ',' + zhuDuiName + ":" + keDuiName+',';
                    //遍历盘口
                    for (var k = 0; k < arr[3][5].length; k++) {
                        //根据id判断
                        if (biSaiIdNew == arr[3][5][k][1][0]) {
                            if (arr[3][5][k][1][1] == 1) {                             
                                showString += '|' + arr[3][5][k][2][0] + ',' + arr[3][5][k][2][1] + ',' + arr[3][5][k][1][4] + ',' + arr[3][5][k][1][3]+'|';
                            }
                               
                        }
						else if(arr[3][5][k][1][1] == 3)
						{
							showString+='['+arr[3][5][k][2][0]+","+ arr[3][5][k][2][1]+',' + arr[3][5][k][1][4] +']'; //主客场	
						}
						else if(arr[3][5][k][1][1] == 7)
							{
								showString+='('+arr[3][5][k][2][0]+","+ arr[3][5][k][2][1]+',' + arr[3][5][k][1][4] +')'; //主客场	
							}
						else if(arr[3][5][k][1][1] == 9)
						{
							showString+='{'+arr[3][5][k][2][0]+","+ arr[3][5][k][2][1]+',' + arr[3][5][k][1][4] +'}'; //主客场	
						}
                    }

                    Rows += showString + "\n";
                    
                  
                }
            }     
        }
        return Rows;
    } catch (e) { return e.message.toString();}
    }
   