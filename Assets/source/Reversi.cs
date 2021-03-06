using UnityEngine;
using System.Collections;
/**
* 移植直後
* 	22.8499393463135
*
* おけない場所の思考をスキップ
* 	13.2158613204956
*
* 多次元配列をジャグ配列に変更
*	10.0627851486206
*
*/
/**
* リバーシークラス
*/
public class Reversi {
	private typeOfPiece[] Grid;
	private typeOfPiece turn;
	public enum typeOfPiece {Black, White, Empty};
	public delegate void put_callback(int x, int y, typeOfPiece t);
	private put_callback callback = null;
/**
* コンストラクタ
*/
	public Reversi() {
		int i,j;
		Grid = new typeOfPiece[64];
		for(i=0;i<8;i++) {
			for(j=0;j<8;j++) {
				Grid[i+j*8] = typeOfPiece.Empty;
			}
		}
	}
	public void setCallback(put_callback a) {
		callback = a;
	}
	
/**
* 初期化
*/
	public void init() {
		putMark(3, 3, typeOfPiece.Black);
		putMark(4, 4, typeOfPiece.Black);
		putMark(3, 4, typeOfPiece.White);
		putMark(4, 3, typeOfPiece.White);
		turn = typeOfPiece.Black;
	}
/**
* データコピー
*/
	public void copy(typeOfPiece[] gr,typeOfPiece tr) {
		for(int x = 0;x < 8; x++) {
			for(int y = 0;y < 8; y++) {
				Grid[x+y*8] = gr[x+y*8];
			}
		}
		turn = tr;
	}
/**
* 
*/
	private void putMark(int x,int y,typeOfPiece typ) {
		if(callback != null) {
			callback(x,y,typ);
		}
		Grid[x+y*8] = typ;
	}
/**
* クローン
*/
	public Reversi Clone() {
		Reversi ret = new Reversi();
		ret.copy(this.Grid, this.turn);
		return(ret);
	}
/**
*
*/
	private typeOfPiece rev(typeOfPiece bw){
		if(typeOfPiece.Black == bw) {
			return(typeOfPiece.White);
		}
		if(typeOfPiece.White == bw) {
			return(typeOfPiece.Black);
		}
		return(typeOfPiece.Empty);
	}
/**
* argorism of AI
* thinking the best choice
*/
	private int think_core(int think_depth,ref int x,ref int y) {
		if(numOfEmpty() == 0 ) {
			return(-numOf(turn));
		}
		if(think_depth <= 0) {
			return(-evaluate());
		}

		int nRet;
		Reversi tmp = new Reversi();
		tmp.copy(this.Grid, this.turn);
		if(tmp.pass()) {
			if(tmp.pass()) {
				if( tmp.numOf(turn) - tmp.numOf(rev(turn)) > 0 ) {
					nRet = 999999;
				} else {
					nRet = -999999;
				}
			} else {
				int xt=0,yt=0;
				nRet = -tmp.think_core(think_depth - 1,ref xt,ref yt);
			}
		} else {
			int point;
			int max_point=-9999999;
			int xt=0, yt=0;
			for(int i=0; i<8; i++) {
				for(int j=0; j<8; j++) {
					if(canPlacePiece(i, j) == false) {
						continue;
					}
					tmp.copy(this.Grid, this.turn);
					if(tmp.placePiece(i, j)) {
						point = tmp.think_core(think_depth -1,ref xt,ref yt);
						if( max_point < point ) {
							max_point = point;
							x = i;
							y = j;
							//Debug.Log("point:"+point+" x:"+i+" y:"+j);
							//tmp.printByAscii();
						}
					}
				}
			}
			nRet = -max_point;
		}
		return(nRet);
	}
/**
* 埋まってないマス数取得
*/
	public int numOfEmpty(){
		int num=0;
		for(int i=0; i<8; i++) {
			for(int j=0; j<8; j++) {
				if(typeOfPiece.Empty == Grid[i+j*8]){
					num++;
				}
			}
		}
		return(num);
	}
/**
* evaluate current situation
* if turn is Black, it is good for Black pint is higher 
*/
	public int evaluate() {
		int point = 0;
		point += 50 * numOfCanPlacePieces();
		if(point == 0) {
			point -= 10000;
		}

		for(int i=-1; i<=1; i+=2) {
			for(int j=-1; j<=1; j+=2){
				if(Grid[(7*(1+i)/2)+(7*(1+j)/2)*8] == turn) {
					point +=2000; // kado
				}
				if(Grid[(7*(1+i)/2)+(7*(1+j)/2)*8] == rev(turn) ) {
					point -=2000; // kado
				}
				if(Grid[(7*(1+i)/2)+(7*(1+j)/2)*8] == typeOfPiece.Empty) {
					if(Grid[(7*(1+i)/2-i)+(7*(1+j)/2-j)*8] == turn ) {
						point -= 150;
					}
					if(Grid[(7*(1+i)/2-i)+(7*(1+j)/2-j)*8] == rev(turn) ) {
						point += 150;
					}
					if(Grid[(7*(1+i)/2-i)+(7*(1+j)/2-j)*8] == typeOfPiece.Empty) {
						if(Grid[(7*(1+i)/2-2*i)+(7*(1+j)/2-2*j)*8] == turn ) {
							point += 50;
						}
						if(Grid[(7*(1+i)/2-2*i)+(7*(1+j)/2-2*j)*8] == rev(turn) ) {
							point -= 50;
						}
					}
				}
			}
		}

		return(point);
	}
/**
* おける場所数を取得
*/
	public int numOfCanPlacePieces(){
		int num=0;
		for(int i=0; i<8; i++) {
			for(int j=0; j<8; j++) {
				if(canPlacePiece(i, j)) {
					num++;
				}
			}
		}
		return num;
	}
/**
* 置くことが可能か？
*/
	public bool canPlacePiece(int x, int y,bool bLog=false){
		if( !(0<=x && x<8 && 0<=y && y<8) ) {
			return(false);
		}
		if(Grid[x+y*8] != typeOfPiece.Empty) {
			return(false);
		}
		int i;
		int j;
		for(int ip=-1; ip<=1; ip++){
			for(int jp=-1; jp<=1; jp++) {
				if(ip==0 && jp==0) {
					continue;
				}
				if( 0<=x+2*ip && x+2*ip<8 && 0<=y+2*jp && y+2*jp<8 ) {
					int k=1;
					for(i=x+ip, j=y+jp; rev(turn)==Grid[i+j*8];) {
						i+=ip;
						j+=jp;
						k++;
						if(bLog) {
							Debug.Log("canPlacePiece() x:"+i+" y:"+j+" ["+ip+"]["+jp+"]["+turn+"]["+Grid[i+j*8]+"]");
						}
						if(!(0<=i && i<8 && 0<=j && j<8)) {
							break;
						}
						if(typeOfPiece.Empty == Grid[i+j*8]) {
							break;
						}
						if( turn==Grid[i+j*8] && k>=2 ){
							return(true);
						}
					}
				}
			}
		}
		return(false);
	}
/**
* パス
*/
	public bool pass(){
		if( 0==numOfCanPlacePieces() ) {
			turn = rev(turn);
			return(true);
		}else{
			return(false);
		}
	}
/**
* 駒を配置後、すべてひっくり返す
*/
	public bool placePiece(int x, int y) {
		bool all_flag = false;
		if(!(0<=x && x<8 && 0<=y && y<8) ) {
			return(all_flag);
		}
		if(!(typeOfPiece.Empty == Grid[(x+y*8)])) {
			return(all_flag);
		}
		int i;
		int j;
		for(int ip=-1; ip<=1; ip++) {
			for(int jp=-1; jp<=1; jp++) {
				if(ip==0 && jp==0) {
					continue;
				}
				if( 0<=x+2*ip && x+2*ip<8 && 0<=y+2*jp && y+2*jp<8 ) {
					int k=1;
					for(i=x+ip, j=y+jp; rev(turn)== Grid[i+j*8];) {
						i+=ip;
						j+=jp;
						k++;
						if(!(0<=i && i<8 && 0<=j && j<8)) {
							break;
						}
						if(typeOfPiece.Empty == Grid[i+j*8]) {
							break;
						}
						if( turn==Grid[i+j*8] && k>=2 ) {
							all_flag = true;
							putMark(x, y, turn);
							for( i-=ip, j-=jp; turn != Grid[i+j*8]; i-=ip, j-=jp ) {
								putMark(i, j, turn);
							}
							break;
						}
					}
				}
			}
		}
		if(all_flag == true) {
			turn = rev(turn);
		}
		return(all_flag);
	}
/**
*
*/
	public int numOf(typeOfPiece typ){
		int num = -1;
		switch(typ) {
		case typeOfPiece.Black:
			num = numOfBlack();
			break;
		case typeOfPiece.White:
			num = numOfWhite();
			break;
		case typeOfPiece.Empty:
			num = numOfEmpty();
			break;
		default:
			break;
		}
		return(num);
	}
/**
* 黒の数取得
*/
	public int numOfBlack(){
		//Debug.Log("Reversi.numOfBlack() step1");
		int num=0;
		for(int i=0; i<8; i++) {
			for(int j=0; j<8; j++) {
				if(typeOfPiece.Black == Grid[i+j*8]) {
					num++;
				}
			}
		}
		return num;
	}
/**
* 白の数取得
*/
	public int numOfWhite(){
		int num=0;
		for(int i=0; i<8; i++) {
			for(int j=0; j<8; j++) {
				if(typeOfPiece.White == Grid[i+j*8]) {
					num++;
				}
			}
		}
		return num;
	}
/**
* Initialize of argorism of AI
*/
	public int think(int think_depth,ref int x,ref int y) {
		if(numOfEmpty() <= think_depth * 2 ) {
			think_depth *=2;
		}
		int nRet = think_core(think_depth,ref x,ref y);
		return(nRet);
	}
/**
* 文字列変換
*/
	public static explicit operator string (Reversi a) {
		string s;
		s =  "  0 1 2 3 4 5 6 7\n";
		s += "  a b c d e f g h\n";
		for(int i=0; i<8; i++) {
			s += i;
			for(int j=0; j<8; j++){
				if( typeOfPiece.Black == a.statusIs(j, i) ) {
					s +=  "○";
				} else if( typeOfPiece.White == a.statusIs(j, i) ) {
					s +=  "●";
				} else {
					s +=  "・";
				}
			}
			s += i;
			s += "\n";
		}
		s += "  a b c d e f g h\n";
		s += "○:" + a.numOfBlack() + "\n";
		s += "●:" + a.numOfWhite() + "\n";
		return(s);
	}
/**
* 指定位置の駒を取得
*/
	public typeOfPiece statusIs(int x, int y) {
		return(Grid[x+y*8]);
	}
/**
* ターン取得
*/
	public typeOfPiece turnIs() {
		return(turn);
	}
}
