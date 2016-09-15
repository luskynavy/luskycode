package main

import "fmt"
import "math"
import "time"

func main() {

	const STEPS_MAX = 9000

	var steps float64 = STEPS_MAX	
	var xmin float64 = -2
	var ymin float64 = -1
	var xmax float64 = 1
	var ymax float64 = 1
	var xstep float64 = (xmax - xmin) / steps
	var ystep float64 = (ymax - ymin) / steps
	
	var t = time.Now()
	
	//var starttime float64 = CurrentSecond();
	var total int = 0
	
	var mandel [STEPS_MAX * STEPS_MAX]int
	
	
	
	var xmandel int = 0
	var ymandel int = 0
	var x float64
	for x = xmin; xmandel < STEPS_MAX; x += xstep {
		ymandel = 0;
		//fmt.Println(x)
		var y float64
		for y = ymin; ymandel < STEPS_MAX; y += ystep {
			var  x1 float64  = 0
			var  y1 float64 = 0
			var looper int = 0
			for ;looper < 100 && math.Sqrt((x1 * x1) + (y1 * y1)) < 2;
			{
				looper++				
				var xx float64 = (x1 * x1) - (y1 * y1) + x
				y1 = 2 * x1 * y1 + y
				x1 = xx
			}
			total += looper
			mandel[xmandel + ymandel * STEPS_MAX] = looper
			ymandel++
		}
		xmandel++
	}
	
	//var time float64 = CurrentSecond() - starttime

	fmt.Println("mandel 500 200", mandel[500 + 200 * STEPS_MAX])
	fmt.Println("total", total)
	//fmt.Println("time", time)
	fmt.Println("time ", time.Since(t).Seconds())
}
