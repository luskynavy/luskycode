#include "mainwindow.h"
#include "ui_mainwindow.h"

#include<sstream>
#include<vector>
#include <cmath>
#include "subsetSum.h"

#include "windows.h"

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    //default values
    ui->values->setText("2.41 3.24 2.09 2.56 3.28 3.88 1.70 4.93 3.30 1.10 2.20");
    ui->wantedSum->setValue(2.41 + 2.09 + 3.3);
}

MainWindow::~MainWindow()
{
    delete ui;
}

//fast and precise windows timer
double CurrentSecond()
{
    LARGE_INTEGER current;
    QueryPerformanceCounter(&current);
    LONGLONG frequency;
    QueryPerformanceFrequency( (LARGE_INTEGER *)&frequency);
    return (double)current.QuadPart / (double)frequency;
}

void MainWindow::on_pushButton_clicked()
{
    //clear the results
    ui->results->setText("");

    //convert the string values to a vector of double
    std::string v = ui->values->toPlainText().toStdString();

    std::stringstream s (v);
    std::vector<double> array;

    double temp;
	//get each double, stop at first unwanted character
    while(s >> temp)
    {
        //debug for values
        //ui->results->setText(ui->results->toPlainText() + QString::number(temp) + " ");
        array.push_back(temp);
    }
	
	//for debug
	//ui->results->setText(ui->results->toPlainText() + "\n\n");
	
    //timer start
    double startTime = CurrentSecond();

    //search solutions and display them
    ui->results->setText(ui->results->toPlainText() + SubsetSum::search(array, ui->wantedSum->value()));

    //get elapsed time
    double elapsedTime = CurrentSecond() - startTime;

    //display number of values and time elapsed
    ui->results->setText(ui->results->toPlainText() + "Done for " + QString::number(array.size()) + " values  in " + QString::number(elapsedTime, 'f', 3) + " s");
}
