var map = File.ReadAllLines("input");

var w = map[0].Length;
var h = map.Length;

var c_fill = new bool[w];

var adjust_r = new int[h];
var adjust_c = new int[w];

var gs = new List<(int r, int c)>();

int r=0;
foreach (var row in map) {
    var f_gs = row.Select((c,i) => (c,i)).Where(p => p.c=='#').Select(p => p.i);

    var p_adj = r==0 ? 0 : adjust_r[r-1];
    if(!f_gs.Any()) {
        adjust_r[r] = p_adj+1;
    } else {
        foreach(var g_c in f_gs) {
            gs.Add((r,g_c));
            c_fill[g_c] = true;
        }
        adjust_r[r] = p_adj;
    }

    r++;
}

for(int c=0; c<w; c++) {
    var c_adj = c==0 ? 0 : adjust_c[c-1];
    if(!c_fill[c]) {
        adjust_c[c] = c_adj+1;
    } else {
        adjust_c[c] = c_adj;
    }
}

long total = 0;
int adj_factor = 1000000-1; // set to 1 for part 1

// this counts each pairwise 2x so divide total by 2
foreach (var g1 in gs) {
    foreach (var g2 in gs) {
        if (g1 != g2) {
            int dist = Math.Abs(g1.r+adjust_r[g1.r]*adj_factor - (g2.r+adjust_r[g2.r]*adj_factor)) +
                       Math.Abs(g1.c+adjust_c[g1.c]*adj_factor - (g2.c+adjust_c[g2.c]*adj_factor));
            total+=dist;
        }
    }
}

Console.WriteLine(total/2);