from collections import namedtuple

import pytest
import requests

BASE_PATH = "http://localhost:5126/Account"


@pytest.fixture(autouse=True)
def setup_teardown():
    # Setup
    requests.delete(BASE_PATH)

    yield  # tests run here

    # Teardown


def test_get_with_no_accounts_returns_zero_items():
    response = requests.get(BASE_PATH)
    assert response.status_code == 200
    assert len(response.json()) == 0


def test_create_account_returns_new_account():
    new_account = {"name": "assaf"}
    response = requests.post(BASE_PATH, json=new_account)

    assert response.status_code == 201

    actual_account = response.json()

    assert response.headers['Location'] == f'/Account/{actual_account["id"]}'
    assert actual_account['id'] != 0
    assert actual_account['name'] == new_account['name']
    assert actual_account['balance'] == 0
    assert actual_account['isBlocked'] is False


def test_create_account_and_get_returns_new_account():
    new_account = {"name": "assaf"}
    response = requests.post(BASE_PATH, json=new_account)

    assert response.status_code == 201

    response = requests.get(BASE_PATH)

    assert response.status_code == 200

    actual_accounts = response.json()

    assert len(actual_accounts) == 1

    actual_account = dictionary_to_account(actual_accounts[0])

    assert actual_account.id != 0
    assert actual_account.name == new_account['name']
    assert actual_account.balance == 0
    assert actual_account.isBlocked is False


def dictionary_to_account(dictionary):
    return namedtuple("Account", dictionary.keys())(*dictionary.values())
