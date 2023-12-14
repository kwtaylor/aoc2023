
var field = new List<char[]>();

foreach(var line in File.ReadLines("input")) {
    field.Add([.. line]);
}

int w = field[0].Length;
int h = field.Count;

var states = new Dictionary<string,int>();
bool foundloop=false;

const int loops = 1000000000;

// Part 1 
//Tilt(-1,0);

// Part 2
for (int i=0; i<loops; i++) {
    if(!foundloop) {
        var state = field.Select((s,_)=>new string(s)).Aggregate((s1,s2)=>s1+s2);
        int iter=0;
        if(states.TryGetValue(state, out iter)) {
            Console.WriteLine($"Loop found {iter} to {i}");
            var diff = i-iter;
            while(i+diff < loops) i+=diff;
            Console.WriteLine($"Fast forward to {i}");
            foundloop=true;
        } else {
            states[state] = i;
        }
    }
    Tilt(-1,0);
    Tilt(0,-1);
    Tilt(1,0);
    Tilt(0,1);
}

int load = h;
int total = 0;
foreach (var row in field) {
    total+= row.Where(c=>c=='O').Count()*load;
    Console.WriteLine($"{new string(row)} {total}");
    load--;
}

Console.WriteLine(total);

void Tilt(int dr, int dc) {
    int startr = dr>0 ? h-1 : 0;
    int endr = dr>0 ? -1 : h;
    int startc = dc>0 ? w-1 : 0;
    int endc = dc>0 ? -1 : w;
    int rp = dr >0 ? -1 : 1;
    int cp = dc >0 ? -1 : 1;

    for(int r=startr; r!=endr; r+=rp) {
        for(int c=startc; c!=endc; c+=cp) {
            if(field[r][c]=='O') Slide(r,c,dr,dc);
        }
    }
}

void Slide(int r, int c, int dr, int dc) {
    int nr = r+dr;
    int nc = c+dc;
    field[r][c]='.';
    while(nr>=0 && nr<h && nc>=0 && nc<w && field[nr][nc] == '.') {
        r=nr;
        c=nc;
        nr = r+dr;
        nc = c+dc;
    }
    field[r][c]='O';
}