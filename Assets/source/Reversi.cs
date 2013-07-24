using UnityEngine;
using System.Collections;

/**
* リバーシークラス
*/
public class Reversi : MonoBehaviour {
	private typeOfPiece[,] Grid;
	private GameObject[,] marks;
	private typeOfPiece turn;
	public GameObject prefubMark;
	public enum typeOfPiece {Black, White, Empty};
	public bool mode_think = false;

	public void Awake() {
		//Debug.Log("Reversi.Awake()");
		int i,j;
		Grid = new typeOfPiece[8,8];
		//Debug.Log("Reversi.start() step1");
		for(i=0;i<8;i++) {
			for(j=0;j<8;j++) {
				Grid[i,j] = typeOfPiece.Empty;
			}
		}
	}
/**
* 初期化
*/
	public void init(bool th,typeOfPiece[,] gr,typeOfPiece tr) {
		mode_think = th;
		for(int x = 0;x < 8; x++) {
			for(int y = 0;y < 8; y++) {
				Grid[x,y] = gr[x,y];
			}
		}
		turn = tr;
	}
/**
* 初期化（開始時）
*/
	public void Start () {
		if(mode_think == false) {
			marks = new GameObject[8,8];
		}
		putMark(3, 3, typeOfPiece.Black);
		putMark(4, 4, typeOfPiece.Black);
		putMark(3, 4, typeOfPiece.White);
		putMark(4, 3, typeOfPiece.White);
		turn = typeOfPiece.Black;
	}

/**
* フレーム更新
*/
/*	public void Update () {
		if(Input.GetMouseButtonDown(0)) {
		}
	}
	public void OnMouseDown() {
		Vector3 clickDeltaPosition = Input.mousePosition;
		Ray ray = Camera.main.ScreenPointToRay(clickDeltaPosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)){
			//if( hit.collider.gameObject != gameObject ) { return; }
			Debug.Log(hit.point);
		}
	}*/
/**
* 
*/
	private void putMark(int x,int y,typeOfPiece typ) {
		//Debug.Log("putMark()");
		if(mode_think == false) {
			//Debug.Log("not think mode:"+mode_think);
			//Debug.Log("putMark() x:"+ x + " y:"+y+" "+typ);
			if(Grid[x,y] == typeOfPiece.Empty) {
				marks[x,y] = (GameObject)Instantiate(prefubMark, new Vector3(x*10-30 -5, 2, 40 - (y*10) -5), Quaternion.identity);
			}
			if(typ == typeOfPiece.Black) {
				marks[x,y].transform.rotation = Quaternion.Euler(0, 0, 180);
			} else {
				marks[x,y].transform.rotation = Quaternion.Euler(0, 0, 0);
			}
		}
		Grid[x,y] = typ;
	}
/**
* クローン
*/
	public Reversi Clone() {
		Reversi ret = gameObject.AddComponent<Reversi>();
		ret.init(true, this.Grid, this.turn);
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
		} else {
			int nRet;
			Reversi tmp = gameObject.AddComponent<Reversi>();
			tmp.init(true, this.Grid, this.turn);
			//tmp.printByAscii();
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
				//Debug.Log("start["+think_depth+"]");
				for(int i=0; i<8; i++) {
					for(int j=0; j<8; j++) {
						tmp.init(true, this.Grid, this.turn);
						if(tmp.placePiece(i, j)) {
							point = tmp.think_core( think_depth -1,ref xt,ref yt);
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
				//tmp.printByAscii();
				//Debug.Log("end["+think_depth+"]["+x+"]["+y+"]");
			}
			Destroy(tmp);
			return(nRet);
		}
	}
/**
* 埋まってないマス数取得
*/
	public int numOfEmpty(){
		int num=0;
		for(int i=0; i<8; i++) {
			for(int j=0; j<8; j++) {
				if(typeOfPiece.Empty == Grid[i,j]){
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
				if(Grid[7*(1+i)/2,7*(1+j)/2] == turn) {
					point +=2000; // kado
				}
				if(Grid[7*(1+i)/2,7*(1+j)/2] == rev(turn) ) {
					point -=2000; // kado
				}
				if(Grid[7*(1+i)/2,7*(1+j)/2] == typeOfPiece.Empty) {
					if(Grid[7*(1+i)/2-i,7*(1+j)/2-j] == turn ) {
						point -= 150;
					}
					if(Grid[7*(1+i)/2-i,7*(1+j)/2-j] == rev(turn) ) {
						point += 150;
					}
					if(Grid[7*(1+i)/2-i,7*(1+j)/2-j] == typeOfPiece.Empty) {
						if(Grid[7*(1+i)/2-2*i,7*(1+j)/2-2*j] == turn ) {
							point += 50;
						}
						if(Grid[7*(1+i)/2-2*i,7*(1+j)/2-2*j] == rev(turn) ) {
							point -= 50;
						}
					}
				}
			}
		}

		return point;
	}
/**
*
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
*
*/
	public bool canPlacePiece(int x, int y,bool bLog=false){
		if( !(0<=x && x<8 && 0<=y && y<8) ) {
			return(false);
		}
		if(Grid[x,y] != typeOfPiece.Empty) {
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
					for(i=x+ip, j=y+jp; rev(turn)==Grid[i,j];) {
						i+=ip;
						j+=jp;
						k++;
						if(bLog) {
							Debug.Log("canPlacePiece() x:"+i+" y:"+j+" ["+ip+"]["+jp+"]["+turn+"]["+Grid[i,j]+"]");
						}
						if(!(0<=i && i<8 && 0<=j && j<8)) {
if(bLog) {
	Debug.Log("step1");
}
							break;
						}
						if(typeOfPiece.Empty == Grid[i,j]) {
if(bLog) {
	Debug.Log("step2");
}
							break;
						}
						if( turn==Grid[i,j] && k>=2 ){
if(bLog) {
	Debug.Log("step3");
}
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
*
*/
	public bool placePiece(int x, int y) {
		bool all_flag = false;
		if(!(0<=x && x<8 && 0<=y && y<8) ) {
			return(all_flag);
		}
		if(!(typeOfPiece.Empty == Grid[x,y])) {
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
					for(i=x+ip, j=y+jp; rev(turn)== Grid[i,j];) {
						i+=ip;
						j+=jp;
						k++;
						if(!(0<=i && i<8 && 0<=j && j<8)) {
							break;
						}
						if(typeOfPiece.Empty == Grid[i,j]) {
							break;
						}
						if( turn==Grid[i,j] && k>=2 ) {
							all_flag = true;
							putMark(x, y, turn);
							for( i-=ip, j-=jp; turn != Grid[i,j]; i-=ip, j-=jp ) {
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
*
*/
	public int numOfBlack(){
		//Debug.Log("Reversi.numOfBlack() step1");
		int num=0;
		for(int i=0; i<8; i++) {
			for(int j=0; j<8; j++) {
				if(typeOfPiece.Black == Grid[i,j]) {
					num++;
				}
			}
		}
		return num;
	}
/**
*
*/
	public int numOfWhite(){
		int num=0;
		for(int i=0; i<8; i++) {
			for(int j=0; j<8; j++) {
				if(typeOfPiece.White == Grid[i,j]) {
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
*
*/
	public void printByAscii() {
		string s;
		s =  "  0 1 2 3 4 5 6 7\n";
		s += "  a b c d e f g h\n";
		for(int i=0; i<8; i++) {
			s += i;
			for(int j=0; j<8; j++){
				if( typeOfPiece.Black == statusIs(j, i) ) {
					s +=  "●";
				} else if( typeOfPiece.White == statusIs(j, i) ) {
					s +=  "○";
				} else {
					s +=  "・";
				}
			}
			s += i;
			s += "\n";
		}
		s += "  a b c d e f g h";
		Debug.Log(s);
	}
/**
*
*/
	public typeOfPiece statusIs(int x, int y) {
		return(Grid[x,y]);
	}
/**
*
*/
	public typeOfPiece turnIs() {
		return(turn);
	}
/*
void readTextPiecesData(string name);
*/


/*
int numOf(typeOfPiece typ);
int numOfEmpty();
int evaluate();
bool pass();
bool placePiece(int x, int y);
int numOfCanPlacePieces();
bool canPlacePiece(int x, int y);
int numOfBlack();
int numOfWhite();
int think(int think_depth, int &x, int &y);

void printByAscii();
typeOfPiece statusIs(int x, int y){return Grid[x][y];}
typeOfPiece turnIs(){return turn;}
*/
}
