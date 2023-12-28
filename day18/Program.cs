var moves = new List<(char d, int l, string c)>();
int max_r=0;
int min_r=0;
int max_c=0;
int min_c=0;

int cur_r=0;
int cur_c=0;

foreach(var line in File.ReadLines("input")) {
    var split = line.Split(' ');
    var dir = split[0][0];
    var len = int.Parse(split[1]);
    moves.Add((dir, len, split[2]));
    if (dir=='R') {
        cur_c+=len;
        if(cur_c>max_c) max_c=cur_c;
    } else if (dir=='U') {
        cur_r-=len;
        if(cur_r<min_r) min_r=cur_r;
    } else if (dir=='L') {
        cur_c-=len;
        if(cur_c<min_c) min_c=cur_c;
    } else if (dir=='D') {
        cur_r+=len;
        if(cur_r>max_r) max_r=cur_r;
    }
}

cur_r=-min_r+1;
cur_c=-min_c+1;
int h = max_r-min_r+3; // add empty space all around for easy flood fill
int w = max_c-min_c+3;

Console.WriteLine($"{cur_r} {cur_c} {h} {w}");

var dug=new bool[h,w];

dug[cur_r,cur_c] = true;

foreach(var move in moves) {
    int dr=0;
    int dc=0;
    if (move.d=='R') {
        dc=1;
    } else if (move.d=='U') {
        dr=-1;
    } else if (move.d=='L') {
        dc=-1;
    } else if (move.d=='D') {
        dr=1;
    }
    for(int n=0;n<move.l;n++) {
        cur_r+=dr;
        cur_c+=dc;
        dug[cur_r,cur_c] = true;
    }
}

//Part 1 --BFS
var fillq = new Queue<(int r, int c)>();
fillq.Enqueue((0,0));

int empty_count=0;
var seen=new bool[h,w];
seen[0,0]=true;

while(fillq.Count>0) {
    var loc=fillq.Dequeue();
    if(!dug[loc.r, loc.c]) {
        empty_count++;
        foreach ((int dr, int dc) dir in new[] {(0,1),(0,-1),(1,0),(-1,0),(1,1),(1,-1),(-1,1),(-1,-1)}) {
            var nr=loc.r+dir.dr;
            var nc=loc.c+dir.dc;
            if(nr>=0 && nr<h && nc>=0 && nc<w && !seen[nr,nc]) {
                fillq.Enqueue((nr,nc));
                seen[nr,nc]=true;
            }
        }
    }
}

// for(int r=0; r<h; r++) {
//     for(int c=0; c<w; c++) {
//         Console.Write(dug[r,c] ? '#' : seen[r,c] ? 'S' : '.');
//     }
//     Console.WriteLine();
// }

Console.WriteLine($"Part 1 {w*h-empty_count}");

// Part 2, way simpler
cur_r =0;

long area=0;
long peri=0;

foreach(var m in moves) {
    int len = int.Parse(m.c[2..7],System.Globalization.NumberStyles.HexNumber);
    int new_dir = int.Parse(m.c[7..8]);
    (int dr, int dc) = DirToOffs(new_dir);
    peri += len;
    area += (long)cur_r*dc*len;
    Console.WriteLine($"{cur_r} * {dc*len} = {(long)cur_r*dc*len} => {area}");
    cur_r+=dr*len;
}

// reverse pick's
long squares = long.Abs(area) + peri/2 + 1;

Console.WriteLine($"Part 2 {area} {peri} {squares}");


static (int dr, int dc) DirToOffs(int dir) => dir switch {
    0 => (0,1), // R
    1 => (1,0), // D
    2 => (0,-1), // L
    3 => (-1,0), // U
    _ => (0,0)
};
