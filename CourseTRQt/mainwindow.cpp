#include "mainwindow.h"
#include "ui_mainwindow.h"

#include<sstream>
#include<vector>
#include <cmath>

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

QString MainWindow::subsetSet(std::vector<double> products, double wantedSum)
{
    QString res = "";

    //for each 2^n possibilities
    double max = pow(2, products.size());
    for (int testVal = 0; testVal < max; testVal++)
    //for (int testVal = 0; testVal < Math.Pow(2, products.Length); testVal++)
    {
        double sum = 0;
        for (int choice = 0; choice < products.size(); choice++)
        {
            //if number is selected
            if (((1 << choice) & testVal) == (1 << choice))
            {
                sum += products[choice];
            }
        }

        //if sum is found minus epsilon
        if (abs(sum - wantedSum) < 1e-5)
        {
            //add solution
            for (int j = 0; j < products.size(); j++)
            {
                //if number is selected
                if (((1 << j) & testVal) == (1 << j))
                {
                    res += QString::number(products[j]) + " ";
                }
            }
            res += "\n";
        }
    }

    return res;
}

void MainWindow::on_pushButton_clicked()
{
    //clear the results
    ui->results->setText("");

    //convert the string values to and vector of double
    std::string v = ui->values->toPlainText().toStdString();

    std::stringstream s (v);
    std::vector<double> array;

    double temp;
    while(s >> temp)
    {
        //debug for values
        //ui->results->setText(ui->results->toPlainText() + QString::number(temp) + " ");
        array.push_back(temp);
    }

    //timer start
    double startTime = CurrentSecond();

    //search solutions and display them
    ui->results->setText(ui->results->toPlainText() + subsetSet(array, ui->wantedSum->value()));

    //get elapsed time
    double elapsedTime = CurrentSecond() - startTime;

    //display number of values and time elapsed
    ui->results->setText(ui->results->toPlainText() + "Done for " + QString::number(array.size()) + " values  in " + QString::number(elapsedTime, 'f', 3) + " s");
}
