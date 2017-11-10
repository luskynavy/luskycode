#include "mainwindow.h"
#include "ui_mainwindow.h"


MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    initTableView();
}

void MainWindow::initTableView()
{
    model = new QStandardItemModel(0, 4, this); //2 Rows and 3 Columns
    model->setHorizontalHeaderItem(0, new QStandardItem(QString("Name")));
    model->setHorizontalHeaderItem(1, new QStandardItem(QString("Source")));
    model->setHorizontalHeaderItem(2, new QStandardItem(QString("Dest")));
    model->setHorizontalHeaderItem(3, new QStandardItem(QString("Last")));

    ui->tableView->setModel(model);
}

MainWindow::~MainWindow()
{
    delete ui;
}

QList<QStandardItem *> MainWindow::prepareRow(const QString &p1, const QString &p2, const QString &p3, const QString &p4)
{
    QList<QStandardItem *> rowItems;
    rowItems << new QStandardItem(p1);
    rowItems << new QStandardItem(p2);
    rowItems << new QStandardItem(p3);
    rowItems << new QStandardItem(p4);

    return rowItems;
}

void MainWindow::on_PushButton_Add_clicked()
{
    QList<QStandardItem *> row = prepareRow("truc", "222", "333", "2017-11-10 15:50:10");
    model->appendRow(row);
}

void MainWindow::on_pushButton_Delete_clicked()
{
    QItemSelectionModel *selected = ui->tableView->selectionModel();
    if (selected->hasSelection())
    {
        model->removeRow(selected->selectedRows().at(0).row());
    }
}
