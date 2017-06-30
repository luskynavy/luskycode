package main

import "C"
import "fmt"

//export PrintBye
func PrintBye() {
    fmt.Println("From DLL: Bye!")
}

func main() {
    // Need a main function to make CGO compile package as C shared library
	//PrintBye();
}