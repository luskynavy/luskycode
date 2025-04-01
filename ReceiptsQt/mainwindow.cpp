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

    initDb();

    getGroupList();

    _ready = true;
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::on_searchButton_clicked()
{
    submit();
}


void MainWindow::on_clearButton_clicked()
{
    _ready = false;
    ui->groupComboBox->setCurrentIndex(0);
    ui->nameLineEdit->setText("");
    ui->sortComboBox->setCurrentIndex(0);
    //submit();
    _ready = true;
}

void MainWindow::on_pagesizeComboBox_currentIndexChanged(int index)
{
    if (_ready)
    {
        auto val = ui->pagesizeComboBox->itemData(index);
        _currentPageSize = val.toInt();
        submit();
    }
}

void MainWindow::initDb()
{
    _db = QSqlDatabase::addDatabase("QMYSQL");
    _db.setDatabaseName(_databaseName);
    _db.setHostName(_host);
    _db.setPort(_port);
    if (!_db.open(_user, _password)) {

    }
}

void MainWindow::getGroupList()
{

    QSqlQuery query(_db);
    query.exec("SELECT DISTINCT `group` FROM products ORDER BY `group`");
    ui->groupComboBox->addItem("");
    while (query.next()) {
        QString group = query.value(0).toString();
        ui->groupComboBox->addItem(group);
    }
}

void MainWindow::submit()
{
    _currentGroup = ui->groupComboBox->itemText(ui->groupComboBox->currentIndex());

    _currentName = ui->nameLineEdit->text();

    auto val = ui->sortComboBox->itemData(ui->sortComboBox->currentIndex());
    _currentSort = val.toString();

    qDebug() << "group " << _currentGroup;
    qDebug() << "name " << _currentName;
    qDebug() << "sort " << _currentSort;
    qDebug() << "page " << _currentPage;
    qDebug() << "pagesize " << _currentPageSize;
}
