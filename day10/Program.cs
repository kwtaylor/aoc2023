var map = File.ReadAllLines("input");

var h=map.Length;
var w=map[0].Length;

var edge = new bool[h,w];

var s_r = 0;
var s_c = 0;

foreach (var row in map) {
    s_c = row.IndexOf('S');
    if (s_c >=0) break;
    s_r++;
}

edge[s_r,s_c] = true;

int c_r=0;
int c_c=0;

(bool u, bool d, bool l, bool r) conn_start = (false,false,false,false);

if(Connects(map[s_r-1][s_c]).d) {
    // connect up
    c_r=s_r-1;
    c_c=s_c;
    conn_start.u=true;
}
if(Connects(map[s_r][s_c+1]).l) {
    // connect right
    c_r=s_r;
    c_c=s_c+1;
    conn_start.r=true;
}
if(Connects(map[s_r+1][s_c]).u) {
    // connect down
    c_r=s_r+1;
    c_c=s_c;
    conn_start.d=true;
}
if(Connects(map[s_r][s_c-1]).r) {
    // connect left
    c_r=s_r;
    c_c=s_c-1;
    conn_start.l=true;
}

int dist = 1;
int p_r=s_r;
int p_c=s_c;

while(c_r!=s_r || c_c!=s_c) {
    edge[c_r,c_c] = true;
    var (m_r,m_c) = MoveThru(map[c_r][c_c], p_r-c_r, p_c-c_c);
    p_r=c_r;
    p_c=c_c;
    c_r+=m_r;
    c_c+=m_c;
    dist++;
}

Console.WriteLine($"Part 1: {dist/2}");

int incount=0;

//scan horizontal and toggle inside if edge connects down
for(int r=0; r<h; r++) {
    bool inside=false;
    for(int c=0; c<w; c++) {
        if(edge[r,c]) Console.Write(map[r][c]);
        else if(inside) Console.Write('*');
        else Console.Write(' ');

        if(inside && !edge[r,c]) incount++;

        if(edge[r,c]) 
            if( (map[r][c] != 'S' && Connects(map[r][c]).d) ||
                (map[r][c] == 'S' && conn_start.d)) inside = !inside;
    }
    Console.WriteLine();
}

Console.WriteLine($"Part 2: {incount}");

static (bool u, bool d, bool l, bool r) Connects(char t) => t switch {
    '|' => (true ,true ,false,false),
    '-' => (false,false,true ,true ),
    'L' => (true ,false,false,true ),
    'J' => (true ,false,true ,false),
    '7' => (false,true ,true ,false),
    'F' => (false,true ,false,true ),
     _  => (false,false,false,false)
};

static (int to_r, int to_c) MoveThru(char t, int from_r, int from_c) {
    var c = Connects(t);
    if(from_r >= 0 && c.u) return (-1, 0);
    if(from_r <= 0 && c.d) return ( 1, 0);
    if(from_c >= 0 && c.l) return ( 0,-1);
                           return ( 0, 1);
}