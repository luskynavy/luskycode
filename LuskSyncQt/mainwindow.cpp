#include "mainwindow.h"
#include "ui_mainwindow.h"

#include <QDebug>
#include <QUrl>

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
    QList<QStandardItem *> row = prepareRow("truc" + QString::number(rand() % 100), "E:\\Users\\yvan.kalafatov\\Documents\\My Games\\SteamWorld Dig\\", "/syncback/SteamWorld Dig", "2017-11-10 15:50:10");
    model->appendRow(row);
}

void MainWindow::on_pushButton_Delete_clicked()
{
    QItemSelectionModel *selected = ui->tableView->selectionModel();
    if (selected->hasSelection())
    {
        int rowId = selected->selectedRows().at(0).row();

        model->removeRow(rowId);
    }
}

void MainWindow::on_pushButton_Edit_clicked()
{
    QItemSelectionModel *selected = ui->tableView->selectionModel();
    if (selected->hasSelection())
    {
        int rowId = selected->selectedRows().at(0).row();

        qDebug() << "edit " << rowId;
    }

    //only files
    QDir dir("E:\\Users\\yvan.kalafatov\\Documents\\My Games\\SteamWorld Dig\\");
    localFiles = dir.entryInfoList(QDir::Files);

    //delete model;

    model = new QStandardItemModel(0, 4, this); //2 Rows and 4 Columns
    model->setHorizontalHeaderItem(0, new QStandardItem(QString("Name")));
    model->setHorizontalHeaderItem(1, new QStandardItem(QString("Size")));
    model->setHorizontalHeaderItem(2, new QStandardItem(QString("Date")));
    model->setHorizontalHeaderItem(3, new QStandardItem(QString("Time")));

    ui->tableView->setModel(model);

    for(auto f : localFiles)
    {
        qDebug() << f.fileName() << " " << f.size() << (f.isDir() ? "DIR" : "FIL")
                 << " " << f.lastModified().date().toString("yyyy-MM-dd")
                 << " " << f.lastModified().time().toString();

        QList<QStandardItem *> row = prepareRow(f.fileName(), QString::number(f.size()), f.lastModified().date().toString("yyyy-MM-dd"), f.lastModified().time().toString());
        model->appendRow(row);
    }
    qDebug() << "";
}

void MainWindow::on_pushButton_Launch_clicked()
{
    QItemSelectionModel *selected = ui->tableView->selectionModel();
    if (selected->hasSelection())
    {
        int rowId = selected->selectedRows().at(0).row();

        qDebug() << "launch " << rowId;        
    }

    ftp = new QFtp(this);
    //connect(ftp, SIGNAL(commandStarted(int)), this, SLOT(commandStarted(int)));
    connect(ftp, SIGNAL(listInfo(QUrlInfo)), this, SLOT(addToList(QUrlInfo)));
    //connect(ftp, SIGNAL(commandFinished(int, bool)), this, SLOT(commandFinished(int, bool)));
    connect(ftp, SIGNAL(done(bool)), this, SLOT(done(bool)));

    QUrl url("ftp://test:truc2@localhost:21/");
    if (!url.isValid() || url.scheme().toLower() != QLatin1String("ftp")) {
        ftp->connectToHost(url.host(), 21);
        ftp->login();
    } else {
        ftp->connectToHost(url.host(), url.port(21));

        if (!url.userName().isEmpty())
            ftp->login(QUrl::fromPercentEncoding(url.userName().toLatin1()), url.password());
        else
            ftp->login();
        if (!url.path().isEmpty())
            ftp->cd(url.path());
        ftp->list();
    }    
}

void MainWindow::addToList(const QUrlInfo &urlInfo)
{
    //only files
    if (!urlInfo.isDir())
    {
        qDebug() << urlInfo.name() << " " << urlInfo.size() << (urlInfo.isDir() ? "DIR" : "FIL")
                 << " " << urlInfo.lastModified().date().toString("yyyy-MM-dd")
                 << " " << urlInfo.lastModified().time().toString();

        remoteFiles.append(urlInfo);
    }
}

/*void MainWindow::commandStarted(int id)
{
    qDebug() << id << ftp->currentId();
}

void MainWindow::commandFinished(int id, bool error)
{
    qDebug() << id << error << ftp->currentId();
}*/

void MainWindow::done(bool error)
{
    qDebug() << " " << error;
}
