#include "mainwindow.h"
#include "ui_mainwindow.h"

#include <QSqlQuery>
#include <QSqlQueryModel>

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

    setGroupList();

    _ready = true;

    submit();
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
    submit();
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

QSqlDatabase MainWindow::openDb()
{
    QSqlDatabase db = QSqlDatabase::addDatabase("QMYSQL");
    db.setDatabaseName(_databaseName);
    db.setHostName(_host);
    db.setPort(_port);
    if (!db.open(_user, _password)) {

    }

    return db;
}

void MainWindow::setGroupList()
{
    QSqlDatabase db = openDb();

    QSqlQuery query(db);
    query.exec("SELECT DISTINCT `group` FROM products ORDER BY `group`");
    ui->groupComboBox->addItem("");
    while (query.next()) {
        QString group = query.value(0).toString();
        ui->groupComboBox->addItem(group);
    }

    db.close();
}

int MainWindow::countRequestProducts(QSqlDatabase db)
{
    QSqlQuery *query = new QSqlQuery(db);

    QString request = "SELECT COUNT(*) FROM products";

    request += getFilters();

    query->prepare(request);
    query->bindValue(":group", _currentGroup);
    query->bindValue(":name", QString("%%1%").arg(_currentName));
    query->bindValue(":pageSize", _currentPageSize);
    query->exec();

    if (query->next())
    {
        return query->value(0).toInt();
    }

    return 0;
}

QString MainWindow::getFilters()
{
    QString filters = "";

    //Manage filters
    int nbParams = 0;
    if (_currentGroup != "")
    {
        filters += " WHERE `group` = :group";
        nbParams++;
    }
    if (_currentName != "")
    {
        if (nbParams ==0) {
            filters += " WHERE name LIKE :name";
            nbParams++;
        } else {
            filters += " AND name LIKE :name";
            nbParams++;
        }
    }

    return filters;
}

QString MainWindow::buildRequestProducts()
{
    QString request = "SELECT `group` as Groupe, name as Nom, price as Prix,"
                      " DATE_FORMAT(DateReceipt, '%d/%m/%Y') as 'Date de réception' FROM products";

    //Manage filters
    request += getFilters();

    //Manage sort
    if (_currentSort == "name")
    {
        request += " ORDER BY name";
    }
    else if (_currentSort == "group")
    {
        request += " ORDER BY `group`, name, dateReceipt";
    }
    else if (_currentSort == "dateReceipt")
    {
        request += " ORDER BY dateReceipt DESC";
    }

    request +=  " LIMIT :pageSize OFFSET :offset";

    return request;
}

void MainWindow::submit()
{
    _currentGroup = ui->groupComboBox->itemText(ui->groupComboBox->currentIndex());

    _currentName = ui->nameLineEdit->text();

    auto val = ui->sortComboBox->itemData(ui->sortComboBox->currentIndex());
    _currentSort = val.toString();    

    QSqlDatabase db = openDb();

    int count = countRequestProducts(db);
    _totalPages = (int)std::ceil(count / (double)_currentPageSize);
    if (_currentPage > _totalPages)
    {
        _currentPage = 1;
    }
    int offset = (_currentPage - 1) * _currentPageSize;

    ui->pageLabel->setText(QString::number(_currentPage) + " sur " + QString::number(_totalPages));

    //Update page buttons enabled state
    ui->firstPushButton->setEnabled(_currentPage != 1);
    ui->previousPushButton->setEnabled(_currentPage >= 2);
    ui->nextPushButton->setEnabled(_currentPage <= _totalPages - 1);
    ui->lastPushButton->setEnabled(_currentPage != _totalPages);

    QSqlQueryModel *model = new QSqlQueryModel();

    QSqlQuery *query = new QSqlQuery(db);

    QString request = buildRequestProducts();

    qDebug() << "group " << _currentGroup;
    qDebug() << "name " << _currentName;
    qDebug() << "sort " << _currentSort;
    qDebug() << "page " << _currentPage;
    qDebug() << "pagesize " << _currentPageSize;
    qDebug() << "offset " << offset;
    qDebug() << "request " << request;

    query->prepare(request);
    query->bindValue(":group", _currentGroup);
    query->bindValue(":name", QString("%%1%").arg(_currentName));
    query->bindValue(":pageSize", _currentPageSize);
    query->bindValue(":offset", offset);
    query->exec();

    model->setQuery(std::move(*query));
    ui->tableView->setModel(model);

    db.close();
}

void MainWindow::on_firstPushButton_clicked()
{
    _currentPage = 1;
    submit();
}


void MainWindow::on_previousPushButton_clicked()
{
    _currentPage -= 1;
    if (_currentPage <= 0)
    {
        _currentPage = 1;
    }
    submit();
}


void MainWindow::on_nextPushButton_clicked()
{
    _currentPage += 1;
    if (_currentPage >= _totalPages)
    {
        _currentPage = _totalPages;
    }
    submit();
}


void MainWindow::on_lastPushButton_clicked()
{
    _currentPage = _totalPages;
    submit();
}

