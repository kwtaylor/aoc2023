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

// Part 2, completely different!
var edges = new List<(int r, int c, int l, int d)>();
var tot_turn=0; // total turn so far, positive = right, negative = left
var cur_dir=0; // current direction heading 0=R 1=D 2=L 3=U
bool first=true;
cur_r =0;
cur_c =0;
var first_dir=0;

foreach(var m in moves) {
    int len = int.Parse(m.c[2..7],System.Globalization.NumberStyles.HexNumber);
    int new_dir = int.Parse(m.c[7..8]);
    edges.Add((cur_r,cur_c,len,new_dir));
    if(!first) tot_turn+=DirToTurn(cur_dir,new_dir);
    else first_dir = new_dir;
    first=false;
    (int dr, int dc) = DirToOffs(new_dir);
    //Console.WriteLine((cur_r,cur_c,len,new_dir switch {0=>'R',1=>'D',2=>'L',3=>'U',_=>'?'},tot_turn));
    cur_r+=dr*len;
    cur_c+=dc*len;
    cur_dir=new_dir;

}

Console.WriteLine((cur_r,cur_c,cur_dir switch {0=>'R',1=>'D',2=>'L',3=>'U',_=>'?'},tot_turn));

if(cur_r !=0 || cur_c !=0 || Math.Abs(tot_turn+DirToTurn(cur_dir,first_dir)) != 4) {
    Console.WriteLine("Warning: Unexpected shape");
    Console.ReadLine();
}

//loop through edges looking for rectangles to cut off and add to area
long area=0;

int i = 0;
int prev_i=edges.Count-1;
while(edges.Count>0) {
    (int c_r,int c_c,int c_len,int c_dir) = edges[i];
    (int p_r,int p_c,int p_len,int p_dir) = edges[prev_i];
    int next_i = i+1 >= edges.Count ? 0 : i+1;
    (int n_r,int n_c,int n_len, int n_dir) = edges[next_i];
    int p_turn = DirToTurn(p_dir, c_dir);
    int n_turn = DirToTurn(c_dir, n_dir);
    Console.Write($"Eval {i,2} R{c_r,7} C{c_c,7} L{c_len,6} D{c_dir} \tprev {prev_i,2} R{p_r,7} C{p_c,7} L{p_len,6} D{p_dir} T{p_turn} \tnext {next_i,2} R{n_r,7} C{n_c,7} L{n_len,6} D{n_dir} T{n_turn}:\t");
    bool move_along=false;
    if(tot_turn>0 && p_turn==1 && n_turn==1 ||
       tot_turn<0 && p_turn==-1 && n_turn==-1) {
        // remove rectangle enclosing area and add to total
        int clip_len = ClipRect(c_r, c_c, c_len, c_dir, n_len, n_dir);
        (int n_dr, int n_dc) = DirToOffs(n_dir);
        (int c_dr, int c_dc) = DirToOffs(c_dir);
        if(clip_len < p_len && clip_len < n_len) {
            area+=(long)clip_len*(c_len+1);
            c_r += n_dr*clip_len;
            c_c += n_dc*clip_len;
            n_r = c_r+c_dr*c_len;
            n_c = c_c+c_dc*c_len;
            edges[prev_i] = (p_r,p_c,p_len-clip_len,p_dir);
            edges[i] = (c_r,c_c,c_len,c_dir);
            edges[next_i] = (n_r,n_c,n_len-clip_len,n_dir);
            Console.Write($" Clipped {clip_len}");
            // after clipping, move along or else get stuck in a loop
            move_along=true;

        } else if(p_len==n_len) {
            area+=(long)p_len*(c_len+1);
            c_r += n_dr*p_len;
            c_c += n_dc*p_len;
            edges[i] = (c_r,c_c,c_len,c_dir);
            if(prev_i>next_i) edges.RemoveAt(prev_i);
            edges.RemoveAt(next_i);
            if(prev_i<next_i) edges.RemoveAt(prev_i);
            if(prev_i<i || next_i<i) i--;
            Console.Write($" Remove both {p_len}");
        } else if(p_len<n_len) {
            area+=(long)p_len*(c_len+1);
            c_r += n_dr*p_len;
            c_c += n_dc*p_len;
            n_r = c_r+c_dr*c_len;
            n_c = c_c+c_dc*c_len;
            edges[i] = (c_r,c_c,c_len,c_dir);
            edges[next_i] = (n_r,n_c,n_len-p_len,n_dir);
            edges.RemoveAt(prev_i);
            if(prev_i<i) i--;
            Console.Write($" Remove prev {p_len}");
        } else if(p_len>n_len) {
            area+=(long)n_len*(c_len+1);
            c_r += n_dr*n_len;
            c_c += n_dc*n_len;
            edges[i] = (c_r,c_c,c_len,c_dir);
            edges[prev_i] = (p_r,p_c,p_len-n_len,p_dir);
            edges.RemoveAt(next_i);
            if(next_i<i) i--;
            Console.Write($" Remove next {n_len}");
        }

    } else if (p_turn==0) {
        // combine with previous edge (could do this more efficiently but this works too)
        edges[i] = (p_r,p_c,c_len+p_len,c_dir);
        edges.RemoveAt(prev_i);
        if(prev_i<i) i--;
        Console.Write($" Concat prev {p_len}");
        
    } else if (p_turn==2 || p_turn==-2) {
        // overlapping lines, remove one or both, add dug out area
        if(p_len==c_len) {
            area+=(long)c_len;
            if(prev_i > i) edges.RemoveAt(prev_i);
            edges.RemoveAt(i);
            if(prev_i < i) edges.RemoveAt(prev_i);
            if(prev_i<i) i--;
            i--;
            Console.Write($" Parallel remove both {c_len}");
        } else if(p_len<c_len) {
            (int c_dr, int c_dc) = DirToOffs(c_dir);
            area+=(long)p_len;
            c_r += c_dr*p_len;
            c_c += c_dc*p_len;
            edges[i] = (c_r,c_c,c_len-p_len,c_dir);
            edges.RemoveAt(prev_i);
            if(prev_i<i) i--;
            Console.Write($" Parallel remove prev {p_len}");
        } else if(p_len>c_len) {
            area+=(long)c_len;
            edges[prev_i] = (p_r,p_c,p_len-c_len,p_dir);
            edges.RemoveAt(i);
            i--;
            Console.Write($" Parallel remove current {c_len}");
        }
        if(i<0) {
            i+=edges.Count;
        }
    } else {
        move_along = true;
    }

    if(move_along) {
        // Done processing edge, Move along
        Console.WriteLine($" Total: {area}");

        prev_i = i;
        i++;
        if(i>=edges.Count) i=0;
        if(prev_i==i) break; // degenerate, only one edge left
        if(i==0) Console.ReadLine();
    } else {
        // re-evaluate current edge
        Console.WriteLine($" RERUN Total: {area}");
        if(i==0) prev_i = edges.Count-1;
        else prev_i = i-1;
    }

}

// There's always one corner left
area++;

Console.WriteLine($"Part 2 {area}");

// convert dirs to turn, -1 = left, 1 = right
static int DirToTurn(int old_d, int new_d) {
    int turn = new_d-old_d;
    if(turn==3) turn=-1;
    if(turn==-3) turn=1;
    return turn;
}

static (int dr, int dc) DirToOffs(int dir) => dir switch {
    0 => (0,1), // R
    1 => (1,0), // D
    2 => (0,-1), // L
    3 => (-1,0), // U
    _ => (0,0)
};

// Clips rectangle width to "edges" points 
// when perpendicular side is from r1,c1 length c_len in direction c_dir
// and max width is p_len in direction p_dir
int ClipRect(int r1, int c1, int c_len, int c_dir, int p_len, int p_dir) {
    (int c_dir_r, int c_dir_c) = DirToOffs(c_dir);
    (int p_dir_r, int p_dir_c) = DirToOffs(p_dir);
    int r2 = r1+c_dir_r*c_len;
    int c2 = c1+c_dir_c*c_len;
    int p_h = p_dir_r*p_len;
    int p_w = p_dir_c*p_len;
    if(p_h==0 && r1==r2 || p_h!=0 && r1!=r2 || p_w==0 && c1==c2 || p_w!=0 && c1!=c2)
        throw new ArgumentException($"Rectangle doesn't make sense {r1} {c1} {c_len} {c_dir} {p_len} {p_dir}");
    int min_r = new[] {r1,r2, r1+p_h}.Min();
    int max_r = new[] {r1,r2, r1+p_h}.Max();
    int min_c = new[] {c1,c2, c1+p_w}.Min();
    int max_c = new[] {c1,c2, c1+p_w}.Max();

    int e=0;
    foreach((int r, int c, _, _) in edges) {
        if( (r != min_r && r != max_r || c != min_c && c != max_c) && // exclude box corners
            r>=min_r && r<=max_r && c>=min_c && c<=max_c) {
            if(p_h > 0) {
                p_h = r-min_r;
                max_r = r;
                p_len = Math.Abs(p_h);
            }
            if(p_h < 0) {
                p_h = r-max_r;
                min_r = r;
                p_len = Math.Abs(p_h);
            }
            if(p_w > 0) {
                p_w = c-min_c;
                max_c = c;
                p_len = Math.Abs(p_w);
            }
            if(p_w < 0) {
                p_w = c-max_c;
                min_c = c;
                p_len = Math.Abs(p_w);
            }
            Console.Write($" X{e}");
        }
        e++;
    }

    return p_len;
}
