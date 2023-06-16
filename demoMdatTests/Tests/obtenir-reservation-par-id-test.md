# ObtenirReservationParIdTest

## 1 - Calcule avec des nuitées à haut tarif

Description

``````yaml
mockAppelTableSQLReservation:
  Value: 
    Id: 231
    NumeroLot: 23A
    DateDebut: 2023-06-30
    DateFin: 2023-07-06

expected:
  verify: 
    - type: match
      allowAdditionalProperties: true
      data:
        NombreNuitees: 6
        NombreNuiteesHautTarif: 5
``````

