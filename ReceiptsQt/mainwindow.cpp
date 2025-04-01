#include "mainwindow.h"
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent)
    : QMainWindow(parent)
    , ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    ui->pagesizeComboBox->addItem("10", 10);
    ui->pagesizeComboBox->addItem("20", 20);
    ui->pagesizeComboBox->addItem("100", 100);
    ui->pagesizeComboBox->addItem("Tout", 100000);

    ui->sortComboBox->addItem("Groupe", "group");
    ui->sortComboBox->addItem("Date de réception", "dateReceipt");
    ui->sortComboBox->addItem("Nom", "name");
}

MainWindow::~MainWindow()
{
    delete ui;
}
