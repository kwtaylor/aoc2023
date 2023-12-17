var map = File.ReadAllLines("input");
int h = map.Length;
int w = map[0].Length;

const int MIND = 4;  //0;
const int MAXD = 10; //3;

var seen = new Dictionary<(int r, int c, int dr, int dc, int d), (ValueTuple<int,int,int,int,int> prev, int nl)>();

var queue = new Queue<(int r, int c, int l, int dr, int dc, int d)>();

queue.Enqueue((0,0,0,0,0,0));
int lowest = 9999;
(int r, int c, int dr, int dc, int d) spot = (0,0,0,0,0);

while(queue.Count>0) {
    (int r, int c, int l, int dr, int dc, int d) = queue.Dequeue();
    //Console.WriteLine($"{r},{c},{l},{dr},{dc},{d}");
    //Console.ReadLine();
    foreach ((int dr, int dc) dir in new[] {(0,1),(0,-1),(1,0),(-1,0)}) {
        int nr = r+dir.dr;
        int nc = c+dir.dc;
        int nd = (dr==dir.dr && dc==dir.dc) ? d+1 : 0;
        if(nr>=0 && nr<h && nc>=0 && nc<w && nd<MAXD && (dr==0 || dir.dr != -dr) && (dc==0 || dir.dc != -dc)
           && ((dr==dir.dr && dc==dir.dc) || d>=MIND-1 || (dr==0 && dc==0))) {
            int nl = l+map[nr][nc]-'0';
            if(!seen.TryGetValue((nr,nc,dir.dr,dir.dc,nd), out var s) || nl < s.nl) {
                seen[(nr,nc,dir.dr,dir.dc,nd)] = ((r,c,dr,dc,d),nl);
                if(nr==h-1 && nc==w-1) {
                    //Console.WriteLine($"Found nl {nl} ${(nr,nc,dir.dr,dir.dc,nd)}");
                    if(nl<lowest) {
                        lowest = nl;
                        spot = (nr,nc,dir.dr,dir.dc,nd);
                    }
                } else {
                    queue.Enqueue((nr,nc,nl,dir.dr,dir.dc,nd));
                }
            }
        }
    }
}

var printmap = new char[h,w];

for(int r=0; r<h; r++) {
    for(int c=0; c<w; c++) {
        printmap[r,c] = map[r][c];
    }
}

while (spot.r!=0 || spot.c!=0) {
    printmap[spot.r,spot.c] = (spot.dr,spot.dc) switch {
        (0,1) => '>',
        (0,-1) => '<',
        (1,0) => 'v',
        (-1,0) => '^',
        _ => '?'
    };
    spot = seen[spot].prev;
}

for(int r=0; r<h; r++) {
    for(int c=0; c<w; c++) {
        Console.Write(printmap[r,c]);
    }
    Console.WriteLine();
}

Console.WriteLine(lowest);