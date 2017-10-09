#include "mainwindow.h"
#include "ui_mainwindow.h"

#define _USE_MATH_DEFINES
#include <math.h>

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    computeDistance();
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::on_radioButton1610_clicked()
{
    computeDistance();
}

void MainWindow::on_radioButton169_clicked()
{
    computeDistance();
}

void MainWindow::on_radioButton43_clicked()
{
    computeDistance();
}

void MainWindow::on_size_valueChanged()
{    
    computeDistance();
}

void MainWindow::on_resolutionX_valueChanged()
{    
    computeDistance();
}

void MainWindow::computeDistance()
{
    try
    {
        double diag = ui->size->value(); //diagonal in inches
        double width = ui->resolutionX->value(); //width in pixels
        double ratio;

        //get ratio from radioButtons
        if (ui->radioButton1610->isChecked())
        {
            ratio = 16.0 / 10;
        }
        else if (ui->radioButton169->isChecked())
        {
            ratio = 16.0 / 9;
        }
        else
        {
            ratio = 4.0 / 3;
        }

        double witdhInches = sqrt(diag * diag / (1 + 1 / (ratio * ratio))); //with in inches of screen
        double p = witdhInches * 2.54 / width; //width of a pixel
        double d = p / 2 / tan(M_PI / 180 / 60 / 2); //optimal distance to screen
        ui->distance->setText(QString::number(d, 'f', 2)); //only 2 digits for decimal

        //diagonal size in cm
        ui->sizeCm->setText(QString::number(ui->size->value() * 2.54, 'f', 2) + " cm");
        //width in cm
        ui->widthCm->setText(QString::number(witdhInches * 2.54, 'f', 2) + " cm");
        //height in cm
        ui->heightCm->setText("* " + QString::number(witdhInches / ratio * 2.54, 'f', 2) + " cm");
    }
    catch(std::exception e)
    {

    };

}
